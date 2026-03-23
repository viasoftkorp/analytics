using Viasoft.Analytics.UserBehaviour.Domain.LicenseUsageHistory.Enum;
using Viasoft.Analytics.UserBehaviour.Domain.Services.DateIntervalProviders;

namespace Viasoft.Analytics.UserBehaviour.Domain.LicenseUsageHistory.DTO
{
    public class FilterByGroupingInput
    {
        public Groupings Groupings { get; set; }
        
        public string AdvancedFilter { get; set; }
        public DateInterval DateInterval { get; set; }
        public int? MaxResultCount { get; set; } = 10;
    }
}