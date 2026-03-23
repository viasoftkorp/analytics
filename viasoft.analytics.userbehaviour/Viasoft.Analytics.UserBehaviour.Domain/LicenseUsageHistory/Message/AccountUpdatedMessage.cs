using System;
using Viasoft.Core.ServiceBus.Abstractions;

namespace Viasoft.Analytics.UserBehaviour.Domain.LicenseUsageHistory.Message
{
    [Endpoint("Viasoft.Licensing.LicensingManagement.AccountUpdatedMessage")]
    public class AccountUpdatedMessage : IMessage, IInternalMessage
    {
        public Guid AccountId { get; set; }
        public string CompanyName { get; set; }
    }
}