using AutoMapper;
using Viasoft.Analytics.UserBehaviour.Domain.DTOs.LicenseUsageHistoryIndex;
using Viasoft.Analytics.UserBehaviour.Domain.Entities;

namespace Viasoft.Analytics.UserBehaviour.Host.Mappers
{
    public class UsageHistoryIndexProfile : Profile
    {
        public UsageHistoryIndexProfile()
        {
            CreateMap<LicenseUsageHistoryIndex, LicenseUsageHistoryIndexOutput>();
        }
    }
}