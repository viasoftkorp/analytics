using System;
using System.Collections.Generic;
using System.Linq;
using Nest;
using Viasoft.Analytics.UserBehaviour.Domain.Consts;
using Viasoft.Analytics.UserBehaviour.Domain.Enums;
using Viasoft.Analytics.UserBehaviour.Domain.LicenseUsageHistory.DTO;
using Viasoft.Analytics.UserBehaviour.Domain.LicenseUsageHistory.Enum;
using Viasoft.Core.IoC.Abstractions;

namespace Viasoft.Analytics.UserBehaviour.Domain.LicenseUsageHistory.Service
{
    public class FilterByGroupingEsService: IFilterByGroupingEsService, ITransientDependency
    {
        public List<FilterByGroupingKey> NormalizeEsResult(string aggregationName, Groupings grouping, ISearchResponse<Entities.LicenseUsageHistory> response)
        {
            if (!response.IsValid)
                throw response.OriginalException;
            switch (grouping)
            {
                case Groupings.App:
                    var appNameField = ElasticSearchUtils.GetCamelCaseField(nameof(Entities.LicenseUsageHistory.AppName));
                    var topApps = (BucketAggregate) response.Aggregations[aggregationName];
                    return topApps.Items
                        .Select(i => (KeyedBucket<object>) i)
                        .Select(i =>
                        {
                            var appIdentifier = (string) i.Key;
                            var appNameAggregation = (TopHitsAggregate) i.Values.First();
                            var appNameAggregationHit = (Dictionary<string, object>) appNameAggregation.Documents<object>().First();
                            appNameAggregationHit.TryGetValue(appNameField, out var appNameAsObject);
                            var appName = (string) appNameAsObject;
                            return new FilterByGroupingKey
                            {
                                AppIdentifier = appIdentifier,
                                AppName = appName,
                                Value = i.DocCount != null ? (int) i.DocCount : 0
                            };
                        })
                        .OrderByDescending(app => app.Value)
                        .ToList();
                case Groupings.Domain:
                    var topDomains = (BucketAggregate) response.Aggregations[aggregationName];
                    return topDomains.Items
                        .Select(i => (KeyedBucket<object>) i)
                        .Select(i => new FilterByGroupingKey
                        {
                            Domain = (Domains) Convert.ToInt32(i.Key),
                            Value = i.DocCount != null ? (int) i.DocCount : 0
                        })
                        .OrderByDescending(domain => domain.Value)
                        .ToList();
                case Groupings.Tenant:
                    var topTenants = (BucketAggregate) response.Aggregations[aggregationName];
                    var accountIdEsField = ElasticSearchUtils.GetCamelCaseField(nameof(Entities.LicenseUsageHistory.AccountId));
                    var accountNameEsField = ElasticSearchUtils.GetCamelCaseField(nameof(Entities.LicenseUsageHistory.AccountName));
                    return topTenants.Items
                        .Select(i => (KeyedBucket<object>) i)
                        .Select(i =>
                        {
                            var metadataAggregation = (TopHitsAggregate) i.Values.First();
                            var metadataAggregationHit = (Dictionary<string, object>) metadataAggregation.Documents<object>().First();
                            metadataAggregationHit.TryGetValue(accountIdEsField, out var licensingIdentifierAsObject);
                            var licensingIdentifier = (string) licensingIdentifierAsObject;
                            metadataAggregationHit.TryGetValue(accountIdEsField, out var accountIdAsObject);
                            var accountId = (string) accountIdAsObject;
                            metadataAggregationHit.TryGetValue(accountNameEsField, out var accountNameAsObject);
                            var accountName = (string) accountNameAsObject;
                            return new FilterByGroupingKey
                            {
                                AccountId = !string.IsNullOrEmpty(accountId) ? Guid.Parse(accountId) : Guid.Empty,
                                AccountName = accountName,
                                LicensingIdentifier = !string.IsNullOrEmpty(licensingIdentifier) ? Guid.Parse(licensingIdentifier) : Guid.Empty,
                                Value = i.DocCount != null ? (int) i.DocCount : 0
                            };
                        })
                        .OrderByDescending(tenant => tenant.Value)
                        .ToList();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}