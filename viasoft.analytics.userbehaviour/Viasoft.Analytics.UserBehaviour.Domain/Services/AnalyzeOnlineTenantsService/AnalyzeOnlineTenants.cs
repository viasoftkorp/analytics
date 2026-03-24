using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Viasoft.Analytics.UserBehaviour.Domain.Consts;
using Viasoft.Analytics.UserBehaviour.Domain.DTOs.LicenseUsageStatistics;
using Viasoft.Analytics.UserBehaviour.Domain.Entities;
using Viasoft.Analytics.UserBehaviour.Domain.ExternalServices.LicenseUsage;
using Viasoft.Core.ApiClient;
using Viasoft.Core.DDD.Extensions;
using Viasoft.Core.DDD.Repositories;
using Viasoft.Core.IoC.Abstractions;

namespace Viasoft.Analytics.UserBehaviour.Domain.Services.AnalyzeOnlineTenantsService
{
    public class AnalyzeOnlineTenants : IAnalyzeOnlineTenants, ITransientDependency
    {
        private readonly IApiClientCallBuilder _apiServiceCallBuilder;
        private readonly IRepository<LicenseUsageReporting> _licenseUsageReporting;
        private readonly ILicenseUsageService _licenseUsageService;

        public AnalyzeOnlineTenants(IApiClientCallBuilder apiServiceCallBuilder, IRepository<LicenseUsageReporting> licenseUsageReporting, ILicenseUsageService licenseUsageService)
        {
            _apiServiceCallBuilder = apiServiceCallBuilder;
            _licenseUsageReporting = licenseUsageReporting;
            _licenseUsageService = licenseUsageService;
        }

        public async Task<OnlineTenantCountOutput> GetOnlineTenantsCountAsync()
        {
            var gatewayCall = _apiServiceCallBuilder
                .WithEndpoint(ExternalServicesConsts.CustomerLicensing.LicenseUsageStatistics.GetOnlineTenantCount)
                .WithServiceName(ExternalServicesConsts.CustomerLicensing.ServiceName)
                .WithHttpMethod(HttpMethod.Get)
                .Build();

            return await gatewayCall.ResponseCallAsync<OnlineTenantCountOutput>();
        }

        public async Task<OnlineUserCountOutput> GetOnlineUsersCountAsync(string advancedFilter)
        {
            var gatewayCall = _apiServiceCallBuilder
                .WithEndpoint(ExternalServicesConsts.CustomerLicensing.LicenseUsageStatistics.GetOnlineUserCount + "?advancedFilter=" + advancedFilter)
                .WithServiceName(ExternalServicesConsts.CustomerLicensing.ServiceName)
                .WithHttpMethod(HttpMethod.Get)
                .Build();

            return await gatewayCall.ResponseCallAsync<OnlineUserCountOutput>();
        }

        public async Task<OnlineAppsCountOutput> GetOnlineAppsCountAsync(string advancedFilter)
        {
            var gatewayCall = _apiServiceCallBuilder
                .WithEndpoint(ExternalServicesConsts.CustomerLicensing.LicenseUsageStatistics.GetOnlineAppsCount + "?advancedFilter=" + advancedFilter)
                .WithServiceName(ExternalServicesConsts.CustomerLicensing.ServiceName)
                .WithHttpMethod(HttpMethod.Get)
                .Build();

            return await gatewayCall.ResponseCallAsync<OnlineAppsCountOutput>();
        }

        public async Task<List<LicensesInUsageCountOutput>> CountLicensesInUsageByIdentifier(string advancedFilter)
        {
            var licenseIdentifiers = new List<Guid>();
            if (!string.IsNullOrEmpty(advancedFilter))
            {
                var licenseIdentifiersForReporting = await _licenseUsageService.GetLicenseIdentifiersUsageForReporting(advancedFilter);
                licenseIdentifiers.AddRange(licenseIdentifiersForReporting);
            }
            
            return await _licenseUsageReporting
                .WhereIf(licenseIdentifiers.Any(), l => licenseIdentifiers.Contains(l.LicensingIdentifier))
                .Select(l => new LicensesInUsageCountOutput {StartInterval = l.StartInterval, EndInterval = l.EndInterval,UsageCount = l.UsageCount})
                .GroupBy(l => new { l.StartInterval, l.EndInterval})
                .Select(l => 
                    new LicensesInUsageCountOutput
                    {
                        StartInterval = l.Key.StartInterval,
                        EndInterval = l.Key.EndInterval,
                        UsageCount = l.Sum(u => u.UsageCount)
                        
                    })
                .OrderBy(l => l.StartInterval)
                .ToListAsync();
        }
    }
}