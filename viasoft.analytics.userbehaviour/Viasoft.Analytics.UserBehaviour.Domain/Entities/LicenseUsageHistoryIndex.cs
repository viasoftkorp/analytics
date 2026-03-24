using System;
using System.ComponentModel.DataAnnotations.Schema;
using Viasoft.Core.DDD.Entities;

namespace Viasoft.Analytics.UserBehaviour.Domain.Entities
{
    [Table("licenseusagehistoryindex")]
    public class LicenseUsageHistoryIndex : Entity
    {
        public Guid TenantId { get; set; }
        public bool IsIndexing { get; set; }
        public bool HasEsFailedSinceLastReindex { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public Guid? ReindexIdentifier { get; set; }
        public string FailureStackTrace { get; set; }
    }
}