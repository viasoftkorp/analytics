using System;

namespace Viasoft.Analytics.UserBehaviour.Domain.DTOs.LicenseUsageReporting
{
    public class LicenseUsageReportingOutput
    {
        public Guid LicensingIdentifier {get; set;}
        
        public int UsageCount {get; set;}
    }
}