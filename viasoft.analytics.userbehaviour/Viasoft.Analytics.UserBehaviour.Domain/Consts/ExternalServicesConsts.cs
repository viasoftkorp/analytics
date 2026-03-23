using System;

namespace Viasoft.Analytics.UserBehaviour.Domain.Consts
{
    public static class ExternalServicesConsts
    {
        public static class CustomerLicensing 
        {
            private static string Domain => "Licensing/";
            private static string App => "CustomerLicensing/";
            
            public static string ServiceName => "Viasoft.Licensing.CustomerLicensing";
            private static string Area => $"{Domain}{App}";

            public static class LicenseUsageStatistics
            {
                private static string Path => $"{Area}{nameof(LicenseUsageStatistics)}/";
                public static string GetOnlineTenantCount => $"{Path}{nameof(GetOnlineTenantCount)}";
                public static string GetOnlineUserCount => $"{Path}{nameof(GetOnlineUserCount)}";
                public static string GetOnlineAppsCount => $"{Path}{nameof(GetOnlineAppsCount)}";
                public static string GetLicenseIdentifiersForUsageReporting => $"{Path}{nameof(GetLicenseIdentifiersForUsageReporting)}";
                public static string GetLicenseUsageForReporting => $"{Path}{nameof(GetLicenseUsageForReporting)}";
            }
        }

        public static class Dashboard
        {
            private static string App => nameof(Dashboard) + "/";
            public static string ServiceName => "Viasoft.Dashboard";
            public static string Area => $"{App}";

            public static class Dashboards
            {
                public static string Path => $"{Area}{nameof(Dashboards)}/";
                public static string DashboardVerification => $"{Path}{nameof(DashboardVerification)}";
            }
        }
        
        public static class LicensingManagement
        {
            private static string Domain => "Licensing/";
            private static string App => "LicensingManagement/";
            public static string ServiceName => "Viasoft.Licensing.LicensingManagement";
            private static string Area => $"{Domain}{App}";
            public static class HostTenant
            {
                private static string Path => $"{Area}{nameof(HostTenant)}/";

                public static string GetHostTenantIdFromLicensingIdentifier(Guid licensingIdentifier)
                {
                    return $"{Path}{nameof(GetHostTenantIdFromLicensingIdentifier)}?licensingIdentifier={licensingIdentifier}";
                }
            }
        }
    }
}