using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Viasoft.Analytics.UserBehaviour.Domain.Entities;
using Viasoft.Analytics.UserBehaviour.Host.BackGroundJobs;
using Viasoft.Core.DDD.Repositories;
using Viasoft.Core.Testing;
using Viasoft.Core.Testing.DateTimeProvider;
using Xunit;

namespace Viasoft.Analytics.UserBehaviour.Testing.BackGroundJobs
{
    public class LicenseUsageReportingExclusionBackGroundJobUnitTest: AnalyticsUserBehaviourTestBase
    {
        
        [Fact(DisplayName = "Testa a adição de novos dados para o license reporting")]
        public async Task Test_Add_License_Reporting()
        {
            // prepare data
            var memoryRepositoryFileLog = ServiceProvider.GetService<IRepository<LicenseUsageReporting>>();
            for (int i = 0; i < 10; i++)
            {
                var newLicenseUsage = new LicenseUsageReporting
                {
                    Day = 16,
                    LicensingIdentifier = Guid.NewGuid(),
                    UsageCount = 3,
                    TenantId = Guid.Parse("00000000-0000-0000-0000-000000000000")
                };
                await memoryRepositoryFileLog.InsertAsync(newLicenseUsage, true);
            }
            var nowLicenseUsage =  new LicenseUsageReporting
            {
                Day = 17,
                LicensingIdentifier = Guid.NewGuid(),
                UsageCount = 3,
                TenantId = Guid.Parse("00000000-0000-0000-0000-000000000000")
            };
            await memoryRepositoryFileLog.InsertAsync(nowLicenseUsage, true);
            var mockDateTimeNow = new FakeDateTimeProvider(DateTime.Parse("2020-06-17 12:15:00"));
            var service = GetService(mockDateTimeNow);
            // execute
            await service.ExecuteAsync(null);
            // test
            var expectedResultAfterExclusion = await memoryRepositoryFileLog.CountAsync();
            Assert.Equal(1, expectedResultAfterExclusion);
            var resultInMemory = await memoryRepositoryFileLog.FirstAsync();
            resultInMemory.Should().BeEquivalentTo(nowLicenseUsage);
        }
        
        private LicenseUsageReportingExclusionBackGroundJob GetService(FakeDateTimeProvider mockDateTimeNow)
        {
            var repo = ServiceProvider.GetService<IRepository<LicenseUsageReporting>>();
            return ActivatorUtilities.CreateInstance<LicenseUsageReportingExclusionBackGroundJob>(ServiceProvider, repo, mockDateTimeNow, UnitOfWork);
        }
    }
}