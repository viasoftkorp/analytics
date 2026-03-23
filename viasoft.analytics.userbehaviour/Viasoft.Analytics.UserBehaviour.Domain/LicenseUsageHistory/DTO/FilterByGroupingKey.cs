using System;
using Viasoft.Analytics.UserBehaviour.Domain.Enums;

namespace Viasoft.Analytics.UserBehaviour.Domain.LicenseUsageHistory.DTO
{
    public class FilterByGroupingKey
    {
        public string AppName { get; set; }
        public string AppIdentifier { get; set; }
        public Domains Domain { get; set; }
        public Guid LicensingIdentifier { get; set; }
        public string AccountName { get; set; }
        public Guid AccountId { get; set; }
        public int Value { get; set; }
        public string DomainName => Domain.ToString();
    }
}