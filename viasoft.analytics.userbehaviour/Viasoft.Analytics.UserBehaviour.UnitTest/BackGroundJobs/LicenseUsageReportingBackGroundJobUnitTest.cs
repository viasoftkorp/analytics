using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Viasoft.Analytics.UserBehaviour.Domain.DTOs.LicenseUsageReporting;
using Viasoft.Analytics.UserBehaviour.Domain.Entities;
using Viasoft.Analytics.UserBehaviour.Domain.ExternalServices.LicenseUsage;
using Viasoft.Analytics.UserBehaviour.Host.BackGroundJobs;
using Viasoft.Core.DDD.Repositories;
using Viasoft.Core.MultiTenancy.Abstractions.Tenant;
using Viasoft.Core.Testing;
using Viasoft.Core.Testing.DateTimeProvider;
using Xunit;

namespace Viasoft.Analytics.UserBehaviour.Testing.BackGroundJobs
{
    public class LicenseUsageReportingBackGroundJobUnitTest: AnalyticsUserBehaviourTestBase
    {
        [Fact(DisplayName = "Testa a adição de novos dados para o license reporting")]
        public async Task Test_Add_License_Reporting()
        {
            // prepare data
            var memoryRepositoryFileLog = ServiceProvider.GetService<IRepository<LicenseUsageReporting>>();
            var mockLicenseUsageService = new Mock<ILicenseUsageService>();
            var fakeLicenseIdentifier = Guid.NewGuid();
            var mockDateTimeNow = new FakeDateTimeProvider(DateTime.Parse("2020-06-16 12:15:00"));
            var fakeResult = new List<LicenseUsageReportingOutput>
            {
                new LicenseUsageReportingOutput{ LicensingIdentifier = fakeLicenseIdentifier, UsageCount = 3}
            };
            var expectedResult = new LicenseUsageReporting
            {
                Day = 16,
                LicensingIdentifier = fakeLicenseIdentifier,
                UsageCount = 3,
                StartInterval = mockDateTimeNow.UtcNow().Subtract(TimeSpan.FromMinutes(5)),
                EndInterval = mockDateTimeNow.UtcNow(),
                TenantId = Guid.Parse("00000000-0000-0000-0000-000000000000")
            };
            mockLicenseUsageService.Setup(s => s.GetLicenseUsageForReporting()).ReturnsAsync(fakeResult);
            var service = GetService(mockDateTimeNow, mockLicenseUsageService);
            // execute
            await service.ExecuteAsync(new LicenseUsageReportingJobData());
            // test
            var result = await memoryRepositoryFileLog.FirstAsync();
            var count = mockLicenseUsageService.Invocations.Where(i => i.Method.Name == nameof(ILicenseUsageService.GetLicenseUsageForReporting)).ToList().Count;
            Assert.Equal(1, count);
            result.Should().BeEquivalentTo(expectedResult, obj => obj.Excluding(p => p.Id));
        }
        
        private LicenseUsageReportingBackGroundJob GetService(FakeDateTimeProvider mockDateTimeNow, Mock<ILicenseUsageService> mockLicenseUsageService)
        {
            var mockTenant = new Mock<ICurrentTenant>();
            return ActivatorUtilities.CreateInstance<LicenseUsageReportingBackGroundJob>(ServiceProvider, mockDateTimeNow, mockLicenseUsageService.Object, mockTenant.Object);
        }
    }
}