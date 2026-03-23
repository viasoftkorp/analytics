using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Viasoft.Analytics.UserBehaviour.Domain.DTOs.LicenseUsageReporting;

namespace Viasoft.Analytics.UserBehaviour.Domain.ExternalServices.LicenseUsage
{
    public interface ILicenseUsageService
    {
        Task<List<LicenseUsageReportingOutput>> GetLicenseUsageForReporting();

        Task<List<Guid>> GetLicenseIdentifiersUsageForReporting(string advancedFilter);
    }
}