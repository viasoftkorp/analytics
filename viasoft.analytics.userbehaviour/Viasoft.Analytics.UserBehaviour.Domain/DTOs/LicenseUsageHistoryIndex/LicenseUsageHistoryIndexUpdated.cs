using System;
using Viasoft.PushNotifications.Abstractions.Contracts;

namespace Viasoft.Analytics.UserBehaviour.Domain.DTOs.LicenseUsageHistoryIndex
{
    public class LicenseUsageHistoryIndexUpdated : NotificationUpdate
    {
        public override string UniqueTypeName => "LicenseUsageHistoryIndexUpdated";
        public bool IsIndexing;
        public bool HasEsFailedSinceLastReindex;
        public DateTime? LastModificationTime;
    }
}