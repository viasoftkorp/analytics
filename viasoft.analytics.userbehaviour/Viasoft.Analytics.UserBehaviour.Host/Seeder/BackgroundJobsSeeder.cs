using System;
using System.Threading.Tasks;
using Hangfire;
using Viasoft.Analytics.UserBehaviour.Domain.DTOs.LicenseUsageReporting;
using Viasoft.Core.BackgroundJobs.Abstractions;
using Viasoft.Core.BackgroundJobs.Abstractions.Manager;
using Viasoft.Core.MultiTenancy.Abstractions.Tenant;
using Viasoft.Core.MultiTenancy.Abstractions.Tenant.Store;
using Viasoft.Data.Seeder.Abstractions;

namespace Viasoft.Analytics.UserBehaviour.Host.Seeder
{
    public class BackgroundJobsSeeder: ISeedData
    {
        private readonly IBackgroundJobManager _backgroundJobManager;
        private readonly ITenancyStore _tenancyStore;
        private readonly ICurrentTenant _currentTenant;

        public BackgroundJobsSeeder(IBackgroundJobManager backgroundJobManager, ICurrentTenant currentTenant, ITenancyStore tenancyStore)
        {
            _backgroundJobManager = backgroundJobManager;
            _currentTenant = currentTenant;
            _tenancyStore = tenancyStore;
        }
        
        public async Task SeedDataAsync()
        {
            if (_currentTenant.Id == Guid.Empty)
            {
                return;
            }
            // In this case _currentTenant.Id is identifier in licensing
            var getHostTenant = await _tenancyStore.GetHostTenantAsync(_currentTenant.Id);
            // In this if hostTenantIdOutput.TenantID is TenantId and CurrentTenantId is Identifier
            if (getHostTenant.HostTenantId == _currentTenant.Id)
            {
                await _backgroundJobManager.AddOrUpdateRecurringJobAsync(new LicenseUsageReportingJobData(),
                    Guid.Parse("F068828D-381A-4C67-AE0D-394B05C97460"), "*/5 * * * *", JobIdKeyStrategy.TenantIdOnly);

                await _backgroundJobManager.AddOrUpdateRecurringJobAsync(new LicenseUsageReportingExclusionJob(),
                    Guid.Parse("EE1CDE39-F6FA-40C6-9AF3-6169D6156D5F"), Cron.Daily(), JobIdKeyStrategy.TenantIdOnly, TimeZoneConverter.TZConvert.GetTimeZoneInfo("America/Sao_Paulo"));
            }
        }
    }
}