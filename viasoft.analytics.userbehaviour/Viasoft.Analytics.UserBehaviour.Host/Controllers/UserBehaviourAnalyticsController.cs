using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Viasoft.Analytics.UserBehaviour.Domain.DTOs.LicenseUsageStatistics;
using Viasoft.Analytics.UserBehaviour.Domain.Services.AnalyzeOnlineTenantsService;
using Viasoft.Core.AspNetCore.Controller;

namespace Viasoft.Analytics.UserBehaviour.Host.Controllers
{
    public class UserBehaviourAnalyticsController: BaseController
    {
        private readonly IAnalyzeOnlineTenants _analyzeOnlineTenants;

        public UserBehaviourAnalyticsController(IAnalyzeOnlineTenants analyzeOnlineTenants)
        {
            _analyzeOnlineTenants = analyzeOnlineTenants;
        }

        [HttpGet]
        public async Task<OnlineTenantCountOutput> CountAllOnlineTenants()
        {
            return await _analyzeOnlineTenants.GetOnlineTenantsCountAsync();
        }

        [HttpGet]
        public async Task<OnlineUserCountOutput> CountAllOnlineUsers(string advancedFilter)
        {
            return await _analyzeOnlineTenants.GetOnlineUsersCountAsync(advancedFilter);
        }

        [HttpGet]
        public async Task<OnlineAppsCountOutput> CountAllOnlineApps(string advancedFilter)
        {
            return await _analyzeOnlineTenants.GetOnlineAppsCountAsync(advancedFilter);
        }
        
        [HttpGet]
        public async Task<List<LicensesInUsageCountOutput>> CountLicensesInUsageByIdentifier(string advancedFilter)
        {
            return await _analyzeOnlineTenants.CountLicensesInUsageByIdentifier(advancedFilter);
        }
    }
}