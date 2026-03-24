using System.Threading.Tasks;
using Viasoft.Analytics.UserBehaviour.Domain.Entities;
using Viasoft.Core.DDD.UnitOfWork;
using Viasoft.Core.EntityFrameworkCore.Extensions;
using Viasoft.Data.Seeder.Abstractions;

namespace Viasoft.Analytics.UserBehaviour.Host.Seeder
{
    public class LicenseUsageHistoryIndexSeeder: ISeedData
    {
        private readonly Core.DDD.Repositories.IRepository<LicenseUsageHistoryIndex> _licenseUsageHistoryIndex;
        private readonly IUnitOfWork _unitOfWork;

        public LicenseUsageHistoryIndexSeeder(Core.DDD.Repositories.IRepository<LicenseUsageHistoryIndex> licenseUsageHistoryIndex, IUnitOfWork unitOfWork)
        {
            _licenseUsageHistoryIndex = licenseUsageHistoryIndex;
            _unitOfWork = unitOfWork;
        }

        public async Task SeedDataAsync()
        {
            using (_unitOfWork.Begin())
            {
                await _licenseUsageHistoryIndex.BatchUpdateAsync(index =>
                        new LicenseUsageHistoryIndex
                        {
                            IsIndexing = false,
                            HasEsFailedSinceLastReindex = true,
                            ReindexIdentifier = null
                        },
                    index => index.IsIndexing);
                
                await _unitOfWork.CompleteAsync();
            }
        }
    }
}