using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Viasoft.Analytics.UserBehaviour.Domain.Consts;
using Viasoft.Analytics.UserBehaviour.Domain.DTOs.Dashboard;
using Viasoft.Core.ApiClient;
using Viasoft.Core.MultiTenancy.Abstractions.Tenant;
using Viasoft.Data.Seeder.Abstractions;

namespace Viasoft.Analytics.UserBehaviour.Domain.Seeder
{
    public class DashboardDataSeeder : ISeedData
    {
        private readonly IApiClientCallBuilder _serviceCallBuilder;
        private readonly ICurrentTenant _currentTenant;

        public DashboardDataSeeder(IApiClientCallBuilder serviceCallBuilder, ICurrentTenant currentTenant)
        {
            _serviceCallBuilder = serviceCallBuilder;
            _currentTenant = currentTenant;
        }

        public async Task SeedDataAsync()
        {
            if (_currentTenant.Id == Guid.Empty)
            {
                return;
            }
            
            var gatewayCall = _serviceCallBuilder.WithEndpoint(ExternalServicesConsts.Dashboard.Dashboards.DashboardVerification)
                .WithServiceName(ExternalServicesConsts.Dashboard.ServiceName)
                .WithBody(new DashboardDto
                {
                    ConsumerId = Guid.Parse(DashboardConsts.ConsumerId),
                    SerializedDashboard = Encoding.UTF8.GetBytes(DashboardConsts.DashboardJson)
                })
                .WithHttpMethod(HttpMethod.Post)
                .Build();
            

            await gatewayCall.ResponseCallAsync<HttpResponseMessage>();

        }
    }
}