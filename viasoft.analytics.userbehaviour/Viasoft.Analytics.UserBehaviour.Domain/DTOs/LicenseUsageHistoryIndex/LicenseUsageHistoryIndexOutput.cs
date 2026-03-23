using System;

namespace Viasoft.Analytics.UserBehaviour.Domain.DTOs.LicenseUsageHistoryIndex
{
    public class LicenseUsageHistoryIndexOutput 
    {
        public bool IsIndexing { get; set; }
        public bool HasEsFailedSinceLastReindex { get; set; }
        public DateTime? LastModificationTime { get; set; }
    }
}