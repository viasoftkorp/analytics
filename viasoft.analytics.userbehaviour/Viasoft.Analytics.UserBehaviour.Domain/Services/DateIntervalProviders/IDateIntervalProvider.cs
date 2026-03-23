using System.Threading.Tasks;
using Viasoft.Analytics.UserBehaviour.Domain.LicenseUsageHistory.DTO;

namespace Viasoft.Analytics.UserBehaviour.Domain.Services.DateIntervalProviders
{
    public interface IDateIntervalProvider
    {
        Task<IntervalDto> GetInterval(DateInterval intervalType);
    }
}