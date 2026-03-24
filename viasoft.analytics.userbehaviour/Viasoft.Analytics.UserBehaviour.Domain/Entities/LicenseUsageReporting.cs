using System;
using System.ComponentModel.DataAnnotations.Schema;
using Viasoft.Core.DDD.Entities;

namespace Viasoft.Analytics.UserBehaviour.Domain.Entities
{
    [Table("licenseusagereporting")]
    public class LicenseUsageReporting: Entity
    {
        public Guid LicensingIdentifier { get; set; }
        
        public DateTime StartInterval { get; set; }
        
        public DateTime EndInterval  { get; set; }
        public int UsageCount { get; set; }
        public int Day { get; set; }
        public Guid TenantId { get; set; }
    }
}