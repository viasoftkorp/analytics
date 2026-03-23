using System.Collections.Generic;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Viasoft.Analytics.UserBehaviour.Domain.Seeder;
using Viasoft.Analytics.UserBehaviour.Host.BackGroundJobs;
using Viasoft.Analytics.UserBehaviour.Host.Extensions;
using Viasoft.Analytics.UserBehaviour.Host.HostedServices;
using Viasoft.Analytics.UserBehaviour.Host.Seeder;
using Viasoft.Analytics.UserBehaviour.Infrastructure.PostgreSql.EntityFrameworkCore;
using Viasoft.Core.API.Administration.Extensions;
using Viasoft.Core.API.Authentication.Extensions;
using Viasoft.Core.API.Authorization.Extensions;
using Viasoft.Core.API.Emailing.Extensions;
using Viasoft.Core.API.EmailTemplate.Extensions;
using Viasoft.Core.API.LicensingManagement.Extensions;
using Viasoft.Core.API.Reporting.Extensions;
using Viasoft.Core.API.TenantManagement.Extensions;
using Viasoft.Core.API.UserProfile.Extensions;
using Viasoft.Core.ApiClient.Extensions;
using Viasoft.Core.AspNetCore.Extensions;
using Viasoft.Core.AspNetCore.Provisioning;
using Viasoft.Core.AspNetCore.UnitOfWork;
using Viasoft.Core.Authentication.Proxy.Extensions;
using Viasoft.Core.Authorization.AspNetCore.Extensions;
using Viasoft.Core.Authorization.Proxy.Extensions;
using Viasoft.Core.BackgroundJobs.Extensions;
using Viasoft.Core.BackgroundJobs.PostgreSQL.Extensions;
using Viasoft.Core.Dashboard.Proxy.Extensions;
using Viasoft.Core.DDD.Extensions;
using Viasoft.Core.ElasticSearch;
using Viasoft.Core.Emailing.Extensions;
using Viasoft.Core.EntityFrameworkCore.Extensions;
using Viasoft.Core.EntityFrameworkCore.PostgreSQL.Extensions;
using Viasoft.Core.Identity.AspNetCore.Extensions;
using Viasoft.Core.IoC.Extensions;
using Viasoft.Core.Mapper.Extensions;
using Viasoft.Core.MultiTenancy.AspNetCore.Extensions;
using Viasoft.Core.MultiTenancy.Options;
using Viasoft.Core.Service;
using Viasoft.Core.ServiceBus.AspNetCore.Extensions;
using Viasoft.Core.ServiceBus.PostgreSQL.Extensions;
using Viasoft.Core.ServiceDiscovery.Extensions;
using Viasoft.Core.Storage.Extensions;
using Viasoft.Data.Seeder.Extensions;
using Viasoft.PushNotifications.AspNetCore.Extensions;

namespace Viasoft.Analytics.UserBehaviour.Host
{
    public class Startup
    {
        public static IServiceConfiguration ServiceConfiguration => new ServiceConfiguration
        {
            ServiceName = "Viasoft.Analytics.UserBehaviour",
            Domain = "Analytics",
            App = "UserBehaviour",
            AppIdentifier = "LS03"
        };
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            //migramos o database antes de tudo, portanto esse hosted service PRECISA ser registrado antes de qualquer coisa
            services.AddHostedService<MigrateDatabaseHostedService>();
            services.AddNullEnvironmentAmbientDataCallOptionsResolver();

            services
                .AddAdministrationApi()
                .AddAuthenticationApi()
                .AddEmailTemplateApi()
                .AddLicensingManagementApi()
                .AddReportingApi()
                .AddTenantManagementApi()
                .AddUserProfileApi()
                .AddPersistence(_configuration)
                .AddEfCore<ViasoftAnalyticsUserBehaviourDbContext>(options =>
                {
                    options.MigrationsOptions.MigrationTimeoutInSeconds = 600;
                })
                .AddEfCorePostgreSql()
                .AddServiceBus(options =>
                {
                    options.SagasOptions.Enabled = true;
                    options.TimeoutOptions.Enabled = true;
                }, ServiceConfiguration, _configuration)
                .AddServiceBusPostgreSqlProvider()
                .AddServiceMesh()
                .AddNotification()
                .AddApiClient(_configuration)
                .AddMultiTenancy(MultiTenancyOptions.Default().TenantNotRequired().CompanyNotRequired().EnvironmentNotRequired())
                .EnableLegacyAutomaticSaveChanges()
                .AddAutoMapper()
                .AddDataSeeder<BackgroundJobsSeeder>()
                .AddDataSeeder<LicenseUsageHistoryIndexSeeder>()
                .RegisterDependenciesByConvention()
                .AddUserIdentity()
                .AddElasticSearch(_configuration)
                .AddSeeders(new []
                {
                    typeof(DashboardDataSeeder),
                })
                .AddAuthorizations(_configuration)
                .AddEmailing()
                .AddDashboardProxy()
                .AddAuthorizationProxy()
                .AddAuthenticationProxy()
                .AddBackgroundJobs(ServiceConfiguration, new List<Assembly>
                {
                    typeof(LicenseUsageReportingBackGroundJob).Assembly
                })
                .AddPostgreSqlBackgroundJobs()
                .AddAuthorizationApi()
                .AddEmailingApi()
                .AddDomainDrivenDesign()
                .AspNetCoreDefaultConfiguration(ServiceConfiguration, _configuration);

            services.AddMemoryCache();
        }
        
        public void Configure(IApplicationBuilder app)
        {
            app.AspNetCoreDefaultAppConfiguration()
                .UseProvisioning()
                .UseUnitOfWork()
                .UseEndpoints();
        }
    }
}