using System;

namespace Viasoft.Analytics.UserBehaviour.Domain.Consts
{
    public static class ElasticSearchConsts
    {
        private static string ServiceNamePrefix => "Viasoft.Analytics.UserBehaviour";

        public static int MaxSearchItemsToSearch => 10000;

        public static class Indexes
        {
            public static string LicenseUsageHistory => $"{ServiceNamePrefix}.{nameof(Entities.LicenseUsageHistory)}Index".ToLower();
        }

        public static string GetIndexName(Guid tenantId, Guid? customTenantId = null)
        {
            if(customTenantId.HasValue && customTenantId.Value != Guid.Empty)
                return $"{customTenantId.Value}-{Indexes.LicenseUsageHistory}";
            if(tenantId == Guid.Empty)
                throw new Exception("TenantId not found");
            return $"{tenantId}-{Indexes.LicenseUsageHistory}";
        }

        public static class UsageHistory
        {
            public static string IndexDefinition => "{\"settings\":{\"analysis\":{\"normalizer\":{\"lowercase_normalizer\":{\"type\":\"custom\",\"filter\":[\"lowercase\"]}}}},\"mappings\":{\"properties\":{\"endTime\":{\"type\":\"date\"},\"accountId\":{\"type\":\"text\",\"fields\":{\"keyword\":{\"type\":\"keyword\",\"ignore_above\":256},\"raw\":{\"type\":\"keyword\",\"normalizer\":\"lowercase_normalizer\"}}},\"accountName\":{\"type\":\"text\",\"fields\":{\"keyword\":{\"type\":\"keyword\",\"ignore_above\":256},\"raw\":{\"type\":\"keyword\",\"normalizer\":\"lowercase_normalizer\"}}},\"language\":{\"type\":\"text\",\"fields\":{\"keyword\":{\"type\":\"keyword\",\"ignore_above\":256},\"raw\":{\"type\":\"keyword\",\"normalizer\":\"lowercase_normalizer\"}}},\"localIpAddress\":{\"type\":\"text\",\"fields\":{\"keyword\":{\"type\":\"keyword\",\"ignore_above\":256},\"raw\":{\"type\":\"keyword\",\"normalizer\":\"lowercase_normalizer\"}}},\"hostUser\":{\"type\":\"text\",\"fields\":{\"keyword\":{\"type\":\"keyword\",\"ignore_above\":256},\"raw\":{\"type\":\"keyword\",\"normalizer\":\"lowercase_normalizer\"}}},\"hostName\":{\"type\":\"text\",\"fields\":{\"keyword\":{\"type\":\"keyword\",\"ignore_above\":256},\"raw\":{\"type\":\"keyword\",\"normalizer\":\"lowercase_normalizer\"}}},\"bundleName\":{\"type\":\"text\",\"fields\":{\"keyword\":{\"type\":\"keyword\",\"ignore_above\":256},\"raw\":{\"type\":\"keyword\",\"normalizer\":\"lowercase_normalizer\"}}},\"bundleIdentifier\":{\"type\":\"text\",\"fields\":{\"keyword\":{\"type\":\"keyword\",\"ignore_above\":256},\"raw\":{\"type\":\"keyword\",\"normalizer\":\"lowercase_normalizer\"}}},\"osInfo\":{\"type\":\"text\",\"fields\":{\"keyword\":{\"type\":\"keyword\",\"ignore_above\":256},\"raw\":{\"type\":\"keyword\"}}},\"softwareVersion\":{\"type\":\"text\",\"fields\":{\"keyword\":{\"type\":\"keyword\",\"ignore_above\":256},\"raw\":{\"type\":\"keyword\"}}},\"databaseName\":{\"type\":\"text\",\"fields\":{\"keyword\":{\"type\":\"keyword\",\"ignore_above\":256},\"raw\":{\"type\":\"keyword\",\"normalizer\":\"lowercase_normalizer\"}}},\"browserInfo\":{\"type\":\"text\",\"fields\":{\"keyword\":{\"type\":\"keyword\",\"ignore_above\":256},\"raw\":{\"type\":\"keyword\",\"normalizer\":\"lowercase_normalizer\"}}},\"additionalLicense\":{\"type\":\"boolean\"},\"additionalLicenses\":{\"type\":\"long\"},\"additionalLicensesConsumed\":{\"type\":\"long\"},\"appIdentifier\":{\"type\":\"text\",\"fields\":{\"keyword\":{\"type\":\"keyword\",\"ignore_above\":256},\"raw\":{\"type\":\"keyword\",\"normalizer\":\"lowercase_normalizer\"}}},\"appLicenses\":{\"type\":\"long\"},\"appLicensesConsumed\":{\"type\":\"long\"},\"appName\":{\"type\":\"text\",\"fields\":{\"keyword\":{\"type\":\"keyword\",\"ignore_above\":256},\"raw\":{\"type\":\"keyword\",\"normalizer\":\"lowercase_normalizer\"}}},\"appStatus\":{\"type\":\"long\"},\"cnpj\":{\"type\":\"text\",\"fields\":{\"keyword\":{\"type\":\"keyword\",\"ignore_above\":256},\"raw\":{\"type\":\"keyword\",\"normalizer\":\"lowercase_normalizer\"}}},\"domain\":{\"type\":\"long\"},\"hash\":{\"type\":\"text\",\"fields\":{\"keyword\":{\"type\":\"keyword\",\"ignore_above\":256},\"raw\":{\"type\":\"keyword\",\"normalizer\":\"lowercase_normalizer\"}}},\"id\":{\"type\":\"text\",\"fields\":{\"keyword\":{\"type\":\"keyword\",\"ignore_above\":256},\"raw\":{\"type\":\"keyword\",\"normalizer\":\"lowercase_normalizer\"}}},\"lastUpdate\":{\"type\":\"date\"},\"licensingIdentifier\":{\"type\":\"text\",\"fields\":{\"keyword\":{\"type\":\"keyword\",\"ignore_above\":256},\"raw\":{\"type\":\"keyword\",\"normalizer\":\"lowercase_normalizer\"}}},\"licensingStatus\":{\"type\":\"long\"},\"softwareIdentifier\":{\"type\":\"text\",\"fields\":{\"keyword\":{\"type\":\"keyword\",\"ignore_above\":256},\"raw\":{\"type\":\"keyword\",\"normalizer\":\"lowercase_normalizer\"}}},\"softwareName\":{\"type\":\"text\",\"fields\":{\"keyword\":{\"type\":\"keyword\",\"ignore_above\":256},\"raw\":{\"type\":\"keyword\",\"normalizer\":\"lowercase_normalizer\"}}},\"startTime\":{\"type\":\"date\"},\"user\":{\"type\":\"text\",\"fields\":{\"keyword\":{\"type\":\"keyword\",\"ignore_above\":256},\"raw\":{\"type\":\"keyword\",\"normalizer\":\"lowercase_normalizer\"}}}}}}";
            public static int DefaultItemsToInsertPerRound => 2500;
            public static int DefaultItemsToGetPerRound => 5000;
        }
    }
}