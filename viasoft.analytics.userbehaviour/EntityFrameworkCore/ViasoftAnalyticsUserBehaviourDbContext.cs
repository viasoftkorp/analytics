using System.Linq;
using Microsoft.EntityFrameworkCore;
using Viasoft.Analytics.UserBehaviour.Domain.Entities;
using Viasoft.Core.EntityFrameworkCore.Context;
using Viasoft.Core.Storage.Schema;

namespace Viasoft.Analytics.UserBehaviour.Infrastructure.EntityFrameworkCore
{
    public class ViasoftAnalyticsUserBehaviourDbContext: BaseDbContext
    {
        public ViasoftAnalyticsUserBehaviourDbContext(DbContextOptions options, ISchemaNameProvider schemaNameProvider) : base(options, schemaNameProvider)
        {
        }
        
        public DbSet<LicenseUsageHistory> LicenseUsageHistories { get; set; }
        
        public DbSet<LicenseUsageReporting> LicenseUsageReporting { get; set; }

        public DbSet<LicenseUsageHistoryIndex> LicenseUsageHistoryIndexes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<LicenseUsageHistory>().HasIndex(company => new
            {
                company.LicensingIdentifier,
                company.Hash,
                company.EndTime
            });

            modelBuilder.Entity<LicenseUsageHistory>().HasIndex(company => new
            {
                company.LicensingIdentifier,
                company.EndTime
            });
            
            modelBuilder.Entity<LicenseUsageHistoryIndex>().HasIndex(history => history.TenantId).IsUnique();
            
            modelBuilder.Entity<LicenseUsageHistoryIndex>().Metadata.GetProperties().First(p => p.Name == nameof(LicenseUsageHistoryIndex.FailureStackTrace)).SetMaxLength(null);
        }
    }
}