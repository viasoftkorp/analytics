using System;
using System.Threading.Tasks;
using Viasoft.Analytics.UserBehaviour.Domain.Entities;

namespace Viasoft.Analytics.UserBehaviour.Host.ElasticSearch.Services
{
    public interface IEsUsageHistoryIndexService
    {
        Task Reindex(Guid reindexIdentifier, int page);
        Task<LicenseUsageHistoryIndex> GetHistoryIndex(Guid? customTenantId = null);
        Task<bool> HasIndex();
        Task<bool> SeedHistories();
        Task CreateHistoryIndex(Guid? customTenantId);
    }
}