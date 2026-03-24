using System;

namespace Viasoft.Analytics.UserBehaviour.Domain.DTOs.LicenseUsageStatistics
{
    public class LicensesInUsageCountOutput
    {
        public DateTime StartInterval { get; set; }
        
        public DateTime EndInterval  { get; set; }
        
        public int UsageCount { get; set; }
    }
}