using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Nest;
using Viasoft.Analytics.UserBehaviour.Domain.Consts;
using Viasoft.Analytics.UserBehaviour.Domain.LicenseUsageHistory.DTO;
using Viasoft.Analytics.UserBehaviour.Domain.LicenseUsageHistory.Enum;
using Viasoft.Analytics.UserBehaviour.Domain.Services.DateIntervalProviders;
using Viasoft.Core.IoC.Abstractions;
using Viasoft.Core.MultiTenancy.Abstractions.Tenant;
using DateInterval = Viasoft.Analytics.UserBehaviour.Domain.Services.DateIntervalProviders.DateInterval;

namespace Viasoft.Analytics.UserBehaviour.Domain.LicenseUsageHistory.Service
{
    public class FilterByGroupingService : IFilterByGroupingService, ITransientDependency
    {
        public static string TopAppsAggregationName => "top-most-accessed-apps";
        public static string TopDomainsAggregationName => "top-most-accessed-domains";
        public static string TopTenantsAggregationName => "top-most-accessed-tenants";
        private readonly IElasticClient _elasticClient;
        private readonly ICurrentTenant _currentTenant;
        private readonly IFilterByGroupingEsService _filterByGroupingEsService;
        private readonly IDateIntervalProvider _intervalProvider;

        public FilterByGroupingService(IElasticClient elasticClient, ICurrentTenant currentTenant, IFilterByGroupingEsService filterByGroupingEsService, IDateIntervalProvider intervalProvider)
        {
            _elasticClient = elasticClient;
            _currentTenant = currentTenant;
            _filterByGroupingEsService = filterByGroupingEsService;
            _intervalProvider = intervalProvider;
        }

        public async Task<FilterByGroupingOutput> FilterByGrouping(FilterByGroupingInput input)
        {
            var searchQuery = new SearchDescriptor<Entities.LicenseUsageHistory>()
                .Index(ElasticSearchConsts.GetIndexName(_currentTenant.Id))
                .Take(0); // We don't want the history, but the aggregation of it
            if (!string.IsNullOrEmpty(input.AdvancedFilter))
                searchQuery = searchQuery.Query(q => q.Raw(input.AdvancedFilter));

            var queries = await ProcessDateInterval(searchQuery, input.DateInterval);
            ISearchResponse<Entities.LicenseUsageHistory> response;
            string aggregationName;
            switch (input.Groupings)
            {
                case Groupings.App:
                    var appNameEsField = ElasticSearchUtils.GetCamelCaseField(nameof(Entities.LicenseUsageHistory.AppName));
                    var appIdentifierEsField = ElasticSearchUtils.GetEsFieldKeyword(nameof(Entities.LicenseUsageHistory.AppIdentifier));
                    var bundleIdentifierField = ElasticSearchUtils.GetEsFieldKeyword(nameof(Entities.LicenseUsageHistory.BundleIdentifier));
                    aggregationName = TopAppsAggregationName;
                    searchQuery = searchQuery
                        .Query(q => q.Bool(b => b.Must(queries.ToArray())))
                        .Aggregations(a => a
                            .Terms(aggregationName, c => c
                                .Field(appIdentifierEsField)
                                .Size(input.MaxResultCount)
                                .Aggregations(a2 => a2
                                    .TopHits(appNameEsField, th => th
                                        .Source(s => s.Includes(i => i.Field(appNameEsField).Field(bundleIdentifierField)))
                                        .Size(1)
                                    ))
                            )    
                        );
                    response = await _elasticClient.SearchAsync<Entities.LicenseUsageHistory>(searchQuery);
                    break;
                case Groupings.Domain:
                    var domainEsField = ElasticSearchUtils.GetCamelCaseField(nameof(Entities.LicenseUsageHistory.Domain));
                    aggregationName = TopDomainsAggregationName;
                    searchQuery = searchQuery
                        .Query(q => q.Bool(b => b.Must(queries.ToArray())))
                        .Aggregations(a => a
                            .Terms(aggregationName, c => c
                                .Field(domainEsField)
                                .Size(input.MaxResultCount)
                            )    
                        );
                    response = await _elasticClient.SearchAsync<Entities.LicenseUsageHistory>(searchQuery);
                    break;
                case Groupings.Tenant:
                     var licensingIdentifierEsField = ElasticSearchUtils.GetCamelCaseField(nameof(Entities.LicenseUsageHistory.LicensingIdentifier));
                    var licensingIdentifierKeywordEsField = ElasticSearchUtils.GetEsFieldKeyword(nameof(Entities.LicenseUsageHistory.LicensingIdentifier));
                    const string metadataAggregationName = "metadata";
                    var accountIdEsField = ElasticSearchUtils.GetCamelCaseField(nameof(Entities.LicenseUsageHistory.AccountId));
                    var accountIdKeywordEsField = ElasticSearchUtils.GetEsFieldKeyword(nameof(Entities.LicenseUsageHistory.AccountId));
                    var accountNameEsField = ElasticSearchUtils.GetCamelCaseField(nameof(Entities.LicenseUsageHistory.AccountName));
                    aggregationName = TopTenantsAggregationName;
                    searchQuery = searchQuery
                        .Query(q => q.Bool(b => b.MustNot(m => m
                            .Term(accountIdKeywordEsField, Guid.Empty.ToString())
                        ).Must(queries.ToArray()))).Aggregations(a => a
                        .Terms(aggregationName, c => c
                            .Field(licensingIdentifierKeywordEsField)
                            .Size(input.MaxResultCount)
                            .Aggregations(a2 => a2
                                .TopHits(metadataAggregationName, th => th
                                    .Source(s => s
                                        .Includes(i => i.Fields(licensingIdentifierEsField, accountIdEsField, accountNameEsField)))
                                    .Size(1)
                                ))
                        )    
                    );
                    var tenantTimer = Stopwatch.StartNew();
                    response = await _elasticClient.SearchAsync<Entities.LicenseUsageHistory>(searchQuery);
                    tenantTimer.Stop();
                    Console.WriteLine($"FilterByGrouping by Tenant took {tenantTimer.ElapsedMilliseconds}ms");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return new FilterByGroupingOutput
            {
                Values = _filterByGroupingEsService.NormalizeEsResult(aggregationName, input.Groupings, response)
            };
        }
        private async Task<List<QueryContainer>> ProcessDateInterval(SearchDescriptor<Entities.LicenseUsageHistory> searchQuery, DateInterval dateInterval)
        {
            if (dateInterval == DateInterval.AllTimes)
            {
                return new List<QueryContainer>();
            }
            var startTimeEsField = ElasticSearchUtils.GetCamelCaseField(nameof(Entities.LicenseUsageHistory.StartTime));
            var interval = await _intervalProvider.GetInterval(dateInterval);
            var queryContainers = new List<QueryContainer>();
            queryContainers.Add(new DateRangeQuery
            {
                Field = startTimeEsField,
                GreaterThan = new DateMathExpression(interval.StartDate),
                LessThan = new DateMathExpression(interval.EndDate)
            });
            return queryContainers;
        }
    }
}