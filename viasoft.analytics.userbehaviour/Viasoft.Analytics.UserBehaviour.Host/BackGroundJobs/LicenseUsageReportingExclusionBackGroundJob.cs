using System.Threading.Tasks;
using Viasoft.Analytics.UserBehaviour.Domain.DTOs.LicenseUsageReporting;
using Viasoft.Analytics.UserBehaviour.Domain.Entities;
using Viasoft.Core.BackgroundJobs.Abstractions;
using Viasoft.Core.DateTimeProvider;
using Viasoft.Core.DDD.Repositories;
using Viasoft.Core.DDD.UnitOfWork;
using Viasoft.Core.EntityFrameworkCore.Extensions;


namespace Viasoft.Analytics.UserBehaviour.Host.BackGroundJobs
{
    public class LicenseUsageReportingExclusionBackGroundJob: IBackgroundJob<LicenseUsageReportingExclusionJob>
    {
        private readonly IRepository<LicenseUsageReporting> _licenseUsageReporting;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IUnitOfWork _unitOfWork;
        
        public LicenseUsageReportingExclusionBackGroundJob(IRepository<LicenseUsageReporting> licenseUsageReporting, IDateTimeProvider dateTimeProvider, IUnitOfWork unitOfWork)
        {
            _licenseUsageReporting = licenseUsageReporting;
            _dateTimeProvider = dateTimeProvider;
            _unitOfWork = unitOfWork;
        }
        
        public async Task ExecuteAsync(LicenseUsageReportingExclusionJob input)
        {
            var currentDay= _dateTimeProvider.UtcNow().Day;
            using (_unitOfWork.Begin(options => options.LazyTransactionInitiation = false))
            {
                await _licenseUsageReporting.BatchHardDeleteAsync(l => l.Day != currentDay);
                await _unitOfWork.CompleteAsync();
            }
        }
    }
}