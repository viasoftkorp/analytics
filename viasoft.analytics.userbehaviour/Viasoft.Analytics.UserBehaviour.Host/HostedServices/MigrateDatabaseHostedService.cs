using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Viasoft.Analytics.UserBehaviour.Domain.Entities;
using Viasoft.Core.DDD.Repositories;
using Viasoft.Core.EntityFrameworkCore.Extensions;
using Viasoft.Core.MultiTenancy.Abstractions.Tenant;

namespace Viasoft.Analytics.UserBehaviour.Host.HostedServices
{
    public class MigrateDatabaseHostedService: IHostedService
    {
        private readonly IServiceProvider _serviceProvider;

        public MigrateDatabaseHostedService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                await SetMultiTenancyProperties(scope.ServiceProvider);
                
                //migramos manualmente antes do serviço iniciar porque existe uma migration que cria um indice em uma tabela que tem muitos registros, e isso pode demorar
                var licenseUsageHistories = scope.ServiceProvider.GetRequiredService<IRepository<LicenseUsageHistory>>();
                var dbContext = licenseUsageHistories.GetUnderlyingDbContext();
                
                dbContext.Database.SetCommandTimeout(TimeSpan.FromMinutes(5));
                await dbContext.Database.MigrateAsync();
            }
        }
        
        private static async Task SetMultiTenancyProperties(IServiceProvider serviceProvider)
        {
            var multiTenancyProperties = serviceProvider.GetRequiredService<IMultiTenancyProperties>();
            await multiTenancyProperties.SetMultiTenancyProperties();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}