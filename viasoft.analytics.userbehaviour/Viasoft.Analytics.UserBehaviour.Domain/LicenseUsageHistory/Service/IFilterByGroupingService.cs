using System.Threading.Tasks;
using Viasoft.Analytics.UserBehaviour.Domain.LicenseUsageHistory.DTO;

namespace Viasoft.Analytics.UserBehaviour.Domain.LicenseUsageHistory.Service
{
    public interface IFilterByGroupingService
    {
        Task<FilterByGroupingOutput> FilterByGrouping(FilterByGroupingInput input);
    }
}