using System;
using Viasoft.Analytics.UserBehaviour.Domain.Enums;
using Viasoft.Core.DDD.Application.Dto.Entities;

namespace Viasoft.Analytics.UserBehaviour.Domain.LicenseUsageHistory.DTO
{
    public class LicenseUsageHistoryOutput : IEntityDto
    {
        public Guid Id { get; set; }
        
        public Guid LicensingIdentifier { get; set; }

        public string AppIdentifier { get; set; }
        
        public string AppName { get; set; }

        public string BundleIdentifier { get; set; }
        
        public string BundleName { get; set; }
        
        public string SoftwareName { get; set; }
        
        public string SoftwareIdentifier { get; set; }

        public int AppLicenses { get; set; }
        
        public int AppLicensesConsumed { get; set; }

        public int AdditionalLicenses { get; set; }
        
        public int AdditionalLicensesConsumed { get; set; }
        
        public bool AdditionalLicense { get; set; }
        
        public DateTime StartTime { get; set; }
        
        public DateTime? EndTime { get; set; }
        
        public string User { get; set; }
        
        public string Cnpj { get; set; }
        
        public DateTime LastUpdate { get; set; }
        
        public LicensingStatus LicensingStatus { get; set; }
        
        public LicensedAppStatus AppStatus { get; set; }
        
        public string SoftwareVersion { get; set; }
        
        public string HostName { get; set; }
        
        public string HostUser { get; set; }
        
        public string LocalIpAddress { get; set; }
        
        public string Language { get; set; }
        
        public string OsInfo { get; set; }
        
        public string BrowserInfo { get; set; }
        
        public string DatabaseName { get; set; }
        
        public TimeSpan AccessDuration { get; set; }
        
        public int AccessDurationInMinutes { get; set; }
        
        public string AccountName { get; set; }
        
        public Domains Domain { get; set; }
        
        public string AccessDurationFormatted
        {
            get
            {
                var duration = AccessDuration;
                return Math.Round(duration.TotalHours).ToString().PadLeft(2, '0')
                       + ":" + duration.Minutes.ToString().PadLeft(2, '0')
                       + ":" + duration.Seconds.ToString().PadLeft(2, '0');
            }
            
        }
    }
}