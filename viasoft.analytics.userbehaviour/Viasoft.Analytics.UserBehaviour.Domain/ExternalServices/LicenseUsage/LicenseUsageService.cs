using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Viasoft.Analytics.UserBehaviour.Domain.Consts;
using Viasoft.Analytics.UserBehaviour.Domain.DTOs.LicenseUsageReporting;
using Viasoft.Core.ApiClient;
using Viasoft.Core.IoC.Abstractions;

namespace Viasoft.Analytics.UserBehaviour.Domain.ExternalServices.LicenseUsage
{
    public class LicenseUsageService: ILicenseUsageService, ITransientDependency
    {
        private readonly IApiClientCallBuilder _apiServiceCallBuilder;
        
        public LicenseUsageService(IApiClientCallBuilder apiServiceCallBuilder)
        {
            _apiServiceCallBuilder = apiServiceCallBuilder;
        }
        
        public async Task<List<LicenseUsageReportingOutput>> GetLicenseUsageForReporting()
        {
            var gatewayCall = _apiServiceCallBuilder
                .WithEndpoint(ExternalServicesConsts.CustomerLicensing.LicenseUsageStatistics.GetLicenseUsageForReporting)
                .WithServiceName(ExternalServicesConsts.CustomerLicensing.ServiceName)
                .WithHttpMethod(HttpMethod.Get)
                .Build();

            return await gatewayCall.ResponseCallAsync<List<LicenseUsageReportingOutput>>();
        }
        
        public async Task<List<Guid>> GetLicenseIdentifiersUsageForReporting(string advancedFilter)
        {
            var gatewayCall = _apiServiceCallBuilder
                .WithEndpoint(ExternalServicesConsts.CustomerLicensing.LicenseUsageStatistics.GetLicenseIdentifiersForUsageReporting + "?advancedFilter=" + advancedFilter)
                .WithServiceName(ExternalServicesConsts.CustomerLicensing.ServiceName)
                .WithHttpMethod(HttpMethod.Get)
                .Build();

            return await gatewayCall.ResponseCallAsync<List<Guid>>();
        }
    }
}