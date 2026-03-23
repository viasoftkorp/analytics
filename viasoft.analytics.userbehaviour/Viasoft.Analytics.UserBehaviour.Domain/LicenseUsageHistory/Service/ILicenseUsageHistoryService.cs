using System.Threading.Tasks;
using Viasoft.Analytics.UserBehaviour.Domain.LicenseUsageHistory.DTO;
using Viasoft.Core.DDD.Application.Dto.Paged;

namespace Viasoft.Analytics.UserBehaviour.Domain.LicenseUsageHistory.Service
{
    public interface ILicenseUsageHistoryService
    {
        Task<PagedResultDto<LicenseUsageHistoryOutput>> GetAll(GetAllLicenseUsageHistory input);
    }
}