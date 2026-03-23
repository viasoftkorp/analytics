using AutoMapper;
using Viasoft.Analytics.UserBehaviour.Domain.Entities;
using Viasoft.Analytics.UserBehaviour.Domain.LicenseUsageHistory.DTO;

namespace Viasoft.Analytics.UserBehaviour.Host.Mappers
{
    public class ViasoftAnalyticsUserBehaviourMapperProfile: Profile
    {
        public ViasoftAnalyticsUserBehaviourMapperProfile()
        {
            MapLicenseUsageHistory();
        }

        private void MapLicenseUsageHistory()
        {
            CreateMap<InsertedLicenseUsage, LicenseUsageHistory>();
            CreateMap<LicenseUsageHistory, LicenseUsageHistoryOutput>();
        }
    }
}