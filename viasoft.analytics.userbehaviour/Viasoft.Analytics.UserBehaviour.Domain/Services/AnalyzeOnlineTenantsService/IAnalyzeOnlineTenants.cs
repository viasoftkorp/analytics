using System.Collections.Generic;
using System.Threading.Tasks;
using Viasoft.Analytics.UserBehaviour.Domain.DTOs.LicenseUsageStatistics;

namespace Viasoft.Analytics.UserBehaviour.Domain.Services.AnalyzeOnlineTenantsService
{
    public interface IAnalyzeOnlineTenants
    {
        Task<OnlineTenantCountOutput> GetOnlineTenantsCountAsync();
        Task<OnlineUserCountOutput> GetOnlineUsersCountAsync(string advancedFilter);
        Task<OnlineAppsCountOutput> GetOnlineAppsCountAsync(string advancedFilter);
        Task<List<LicensesInUsageCountOutput>> CountLicensesInUsageByIdentifier(string advancedFilter);
    }
}