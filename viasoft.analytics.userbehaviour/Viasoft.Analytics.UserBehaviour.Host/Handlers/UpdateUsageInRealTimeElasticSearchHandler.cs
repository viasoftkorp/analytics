using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nest;
using Rebus.Handlers;
using Rebus.Retry.Simple;
using Viasoft.Analytics.UserBehaviour.Domain.Consts;
using Viasoft.Analytics.UserBehaviour.Domain.Entities;
using Viasoft.Analytics.UserBehaviour.Domain.LicenseUsageHistory.Message;
using Viasoft.Analytics.UserBehaviour.Host.ElasticSearch.Services;
using Viasoft.Core.MultiTenancy.Abstractions.Tenant.Store;
using Viasoft.Core.ServiceBus.Abstractions;

namespace Viasoft.Analytics.UserBehaviour.Host.Handlers
{
    public class UpdateUsageInRealTimeElasticSearchHandler: IHandleMessages<IFailed<LicenseUsageInRealTimeElasticSearchUpdateMessage>>,
        IHandleMessages<LicenseUsageInRealTimeElasticSearchUpdateMessage>
    {
        private readonly IServiceBus _serviceBus;
        private readonly IEsUsageHistoryIndexService _historyIndexService;
        private readonly IElasticClient _elasticClient;
        private readonly ITenancyStore _tenancyStore;

        public UpdateUsageInRealTimeElasticSearchHandler(IServiceBus serviceBus, IEsUsageHistoryIndexService historyIndexService, IElasticClient elasticClient, ITenancyStore tenancyStore)
        {
            _serviceBus = serviceBus;
            _elasticClient = elasticClient;
            _tenancyStore = tenancyStore;
            _historyIndexService = historyIndexService;
        }

        public async Task Handle(IFailed<LicenseUsageInRealTimeElasticSearchUpdateMessage> message)
        {
            await _serviceBus.DeadLetter(message.ErrorDescription);
        }

        public async Task Handle(LicenseUsageInRealTimeElasticSearchUpdateMessage message)
        {
            var getHostTenant = await _tenancyStore.GetHostTenantAsync(message.LicensingIdentifier);
            var currentIndex = await _historyIndexService.GetHistoryIndex(getHostTenant.HostTenantId);
            if (currentIndex is { IsIndexing: true })
            {
                await _serviceBus.DeferLocal(TimeSpan.FromSeconds(30), message);
                return;
            }
            
            await UpdateHistoriesIndexInElasticSearch(currentIndex.TenantId, message.InsertedUsageHistories, message.MessageHashes, message.DateTimeNow, message.LicensingIdentifier);
        }
        
        private async Task UpdateHistoriesIndexInElasticSearch(Guid tenantId, IReadOnlyCollection<LicenseUsageHistory> insertedUsageHistories, IEnumerable<string> messageHashes,
            DateTime dateTimeNow, Guid licensingIdentifier)
        {
            var indexName = ElasticSearchConsts.GetIndexName(tenantId);
            await _historyIndexService.CreateHistoryIndex(tenantId);
            BulkResponse createResponse = null;
            if (insertedUsageHistories.Any())
            {
                createResponse = await _elasticClient.BulkAsync(b => b
                    .Index(indexName)
                    .CreateMany(insertedUsageHistories)
                );
            }
            // "Bulk Update" in ES
            const string elasticSearchDateTimeNowParamName = "datetimeNow";
            var elasticSearchScript = $"ctx._source.{ElasticSearchUtils.GetCamelCaseField(nameof(LicenseUsageHistory.EndTime))} = params.{elasticSearchDateTimeNowParamName}";
            var elasticSearchScriptParams = new Dictionary<string, object>
            {
                {elasticSearchDateTimeNowParamName, $"{dateTimeNow:s}"}
            };
            var updateResponse = await _elasticClient.UpdateByQueryAsync<LicenseUsageHistory>(s => s
                .Index(indexName)
                .Query(h => h
                    .Bool(b => b
                        .Must(m => m
                            .Term(t => t
                                .Field(ElasticSearchUtils.GetEsFieldKeyword(nameof(LicenseUsageHistory.LicensingIdentifier)))
                                .Value(licensingIdentifier.ToString())
                            )
                        )
                        .MustNot(m =>
                            m.Exists(e => e.Field(ElasticSearchUtils.GetCamelCaseField(nameof(LicenseUsageHistory.EndTime)))),
                             m => 
                                 m.Terms(terms => terms.Field(ElasticSearchUtils.GetCamelCaseField(nameof(LicenseUsageHistory.Hash))).Terms(messageHashes))
                        )
                    )
                )
                .Script(script => script
                    .Source(elasticSearchScript)
                    .Params(elasticSearchScriptParams)
                )
            );
            var hasEsFailed = (createResponse != null && createResponse.Errors) ||
                              (updateResponse == null || updateResponse.Failures.Any() || !updateResponse.IsValid);
            if (hasEsFailed)
            {
                throw updateResponse?.OriginalException ?? new Exception("Failed while updating indexes");
            }
        }
    }
}