using System;
using System.Collections.Generic;
using Viasoft.Core.ServiceBus.Abstractions;

namespace Viasoft.Analytics.UserBehaviour.Domain.LicenseUsageHistory.Message
{
    [Endpoint("Viasoft.Licensing.CustomerLicensing.LicenseUsageInRealTimeElasticSearchUpdate")]
    public class LicenseUsageInRealTimeElasticSearchUpdateMessage: IMessage, IInternalMessage
    {
        public IReadOnlyCollection<Entities.LicenseUsageHistory> InsertedUsageHistories { get; set; }
        public IEnumerable<string> MessageHashes { get; set; }
        public DateTime DateTimeNow { get; set; }
        public Guid LicensingIdentifier { get; set; }

        public LicenseUsageInRealTimeElasticSearchUpdateMessage()
        {
        }

        public LicenseUsageInRealTimeElasticSearchUpdateMessage(IReadOnlyCollection<Entities.LicenseUsageHistory> insertedUsageHistories, IEnumerable<string> messageHashes, DateTime dateTimeNow, Guid licensingIdentifier)
        {
            InsertedUsageHistories = insertedUsageHistories;
            MessageHashes = messageHashes;
            DateTimeNow = dateTimeNow;
            LicensingIdentifier = licensingIdentifier;
        }
    }
}