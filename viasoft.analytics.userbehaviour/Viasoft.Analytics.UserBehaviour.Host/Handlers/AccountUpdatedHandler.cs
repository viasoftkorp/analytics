using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nest;
using Rebus.Handlers;
using Viasoft.Analytics.UserBehaviour.Domain.Consts;
using Viasoft.Analytics.UserBehaviour.Domain.Entities;
using Viasoft.Analytics.UserBehaviour.Domain.LicenseUsageHistory.Message;
using Viasoft.Analytics.UserBehaviour.Host.ElasticSearch.Services;
using Viasoft.Core.DDD.UnitOfWork;
using Viasoft.Core.EntityFrameworkCore.Extensions;
using Viasoft.Core.MultiTenancy.Abstractions.Tenant;
using Viasoft.Core.ServiceBus.Abstractions;

namespace Viasoft.Analytics.UserBehaviour.Host.Handlers
{
    public class AccountUpdatedHandler: IHandleMessages<AccountUpdatedMessage>
    {
        private readonly Core.DDD.Repositories.IRepository<LicenseUsageHistory> _licenseUsageHistories;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IElasticClient _elasticClient;
        private readonly IEsUsageHistoryIndexService _historyIndexService;
        private readonly ICurrentTenant _currentTenant;
        private readonly IServiceBus _serviceBus;

        public AccountUpdatedHandler(Core.DDD.Repositories.IRepository<LicenseUsageHistory> licenseUsageHistories, IUnitOfWork unitOfWork, IElasticClient elasticClient, IEsUsageHistoryIndexService historyIndexService, ICurrentTenant currentTenant, IServiceBus serviceBus)
        {
            _licenseUsageHistories = licenseUsageHistories;
            _unitOfWork = unitOfWork;
            _elasticClient = elasticClient;
            _historyIndexService = historyIndexService;
            _currentTenant = currentTenant;
            _serviceBus = serviceBus;
        }

        public async Task Handle(AccountUpdatedMessage message)
        {
            var currentIndex = await _historyIndexService.GetHistoryIndex();
            if (currentIndex != null && currentIndex.IsIndexing)
            {
                await _serviceBus.DeferLocal(TimeSpan.FromSeconds(10), message);
                return;
            }
            using (_unitOfWork.Begin(opt => opt.LazyTransactionInitiation = false))
            {
                await _licenseUsageHistories.BatchUpdateAsync(
                    l => new LicenseUsageHistory {AccountName = message.CompanyName},
                    p => p.AccountId == message.AccountId);
                await _unitOfWork.CompleteAsync();
            }
            if (currentIndex != null)
                await UpdateHistoriesInElasticSearch(message);
        }

        private async Task UpdateHistoriesInElasticSearch(AccountUpdatedMessage message)
        {
            const string elasticSearchCompanyNameParamName = "companyName";
            var elasticSearchScriptParams = new Dictionary<string, object>
            {
                {elasticSearchCompanyNameParamName, message.CompanyName}
            };
            var elasticSearchScript =
                $"ctx._source.{ElasticSearchUtils.GetCamelCaseField(nameof(LicenseUsageHistory.AccountName))} = params.{elasticSearchCompanyNameParamName}"; 
            await _elasticClient.UpdateByQueryAsync<LicenseUsageHistory>(s => s
                .Index(ElasticSearchConsts.GetIndexName(_currentTenant.Id))
                .Query(h => h
                    .Bool(b => b
                        .Must(m => m
                            .Term(terms =>
                                terms.Field(ElasticSearchUtils.GetEsFieldKeyword(nameof(LicenseUsageHistory.AccountId)))
                                    .Value(message.AccountId))
                        )
                    )
                )
                .Script(script => script
                    .Source(elasticSearchScript)
                    .Params(elasticSearchScriptParams)
                )
            );
        }
    }
}