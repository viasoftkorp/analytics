using System;
using System.Collections.Generic;
using Viasoft.Analytics.UserBehaviour.Domain.LicenseUsageHistory.DTO;
using Viasoft.Core.ServiceBus.Abstractions;

namespace Viasoft.Analytics.UserBehaviour.Domain.LicenseUsageHistory.Message
{
    [Endpoint("Viasoft.Licensing.CustomerLicensing.LicenseUsageInRealTimeInserted")]
    public class LicenseUsageInRealTimeInsertedMessage : IMessage, IInternalMessage
    {
        public Guid LicensingIdentifier { get; set; }
        public Guid AccountId { get; set; }
        public string AccountName { get; set; }
        public List<InsertedLicenseUsage> InsertedLicensesUsages { get; set; }
    }
}