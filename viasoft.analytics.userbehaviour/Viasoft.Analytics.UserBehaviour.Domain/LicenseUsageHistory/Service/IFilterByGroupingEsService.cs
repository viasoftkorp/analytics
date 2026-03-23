using System.Collections.Generic;
using Nest;
using Viasoft.Analytics.UserBehaviour.Domain.LicenseUsageHistory.DTO;
using Viasoft.Analytics.UserBehaviour.Domain.LicenseUsageHistory.Enum;

namespace Viasoft.Analytics.UserBehaviour.Domain.LicenseUsageHistory.Service
{
    public interface IFilterByGroupingEsService
    {
        List<FilterByGroupingKey> NormalizeEsResult(string aggregationName, Groupings grouping,
            ISearchResponse<Entities.LicenseUsageHistory> response);
    }
}