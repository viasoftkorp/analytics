using System;
using System.Threading.Tasks;
using Viasoft.Analytics.UserBehaviour.Domain.DTOs.LicenseUsageReporting;
using Viasoft.Analytics.UserBehaviour.Domain.Entities;
using Viasoft.Analytics.UserBehaviour.Domain.ExternalServices.LicenseUsage;
using Viasoft.Core.BackgroundJobs.Abstractions;
using Viasoft.Core.DateTimeProvider;
using Viasoft.Core.DDD.Repositories;
using Viasoft.Core.DDD.UnitOfWork;
using Viasoft.Core.MultiTenancy.Abstractions.Tenant;

namespace Viasoft.Analytics.UserBehaviour.Host.BackGroundJobs
{
    public class LicenseUsageReportingBackGroundJob: IBackgroundJob<LicenseUsageReportingJobData>
    {
        private readonly ILicenseUsageService _licenseUsageService;
        private readonly IRepository<LicenseUsageReporting> _licenseUsageReporting;
        private readonly ICurrentTenant _currentTenant;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IUnitOfWork _unitOfWork;
        
        public LicenseUsageReportingBackGroundJob(ILicenseUsageService licenseUsageService, IRepository<LicenseUsageReporting> licenseUsageReporting,
            ICurrentTenant currentTenant, IDateTimeProvider dateTimeProvider, IUnitOfWork unitOfWork)
        {
            _licenseUsageService = licenseUsageService;
            _licenseUsageReporting = licenseUsageReporting;
            _currentTenant = currentTenant;
            _dateTimeProvider = dateTimeProvider;
            _unitOfWork = unitOfWork;
        }
        
        public async Task ExecuteAsync(LicenseUsageReportingJobData input)
        {
            var currentDateTime = _dateTimeProvider.UtcNow();
            var day = currentDateTime.Day;
            var licenseUsageList = await _licenseUsageService.GetLicenseUsageForReporting();
            using (_unitOfWork.Begin())
            {
                foreach (var licenseUsage in licenseUsageList)
                {
                    var licenseUsageReporting = new LicenseUsageReporting
                    {
                        StartInterval = currentDateTime.Subtract(TimeSpan.FromMinutes(5)),
                        EndInterval = currentDateTime,
                        LicensingIdentifier = licenseUsage.LicensingIdentifier,
                        UsageCount = licenseUsage.UsageCount,
                        Day = day,
                        TenantId = _currentTenant.Id
                    };
                    await _licenseUsageReporting.InsertAsync(licenseUsageReporting);
                }
                await _unitOfWork.CompleteAsync();
            }
        }
    }
}