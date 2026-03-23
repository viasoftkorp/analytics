using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Viasoft.Analytics.UserBehaviour.Domain.DTOs.LicenseUsageStatistics;
using Viasoft.Analytics.UserBehaviour.Domain.Entities;
using Viasoft.Analytics.UserBehaviour.Domain.ExternalServices.LicenseUsage;
using Viasoft.Analytics.UserBehaviour.Domain.Services.AnalyzeOnlineTenantsService;
using Viasoft.Core.ApiClient;
using Viasoft.Core.DDD.Repositories;
using Viasoft.Core.Testing;
using Xunit;

namespace Viasoft.Analytics.UserBehaviour.Testing.Services
{
    public class AnalyzeOnlineTenantsServiceUnitTest: AnalyticsUserBehaviourTestBase
    {
        [Fact(DisplayName = "Testa o retorno do método de licenças em uso (sem dados salvos)")]
        public async Task Test_License_Usage_Returns_When_No_Have_Data()
        {
            // prepare data
            var mockLicenseUsage = new Mock<ILicenseUsageService>();
            var service = GetService(mockLicenseUsage);
            // execute
            var result = await service.CountLicensesInUsageByIdentifier(null);
            // test
            Assert.Empty(result);
        }
        
        [Fact(DisplayName = "Testa o retorno do método de licenças em uso (com dados salvos e filtro aplicado)")]
        public async Task Test_License_Usage_Returns_When_Have_Data_And_Filter()
        {
            // prepare data
            var memoryRepositoryFileLog = ServiceProvider.GetService<IRepository<LicenseUsageReporting>>();
            var licenseIdentifiers = new List<Guid>{ Guid.NewGuid(), Guid.NewGuid(),Guid.NewGuid()};
            for (int i = 0; i < 3; i++)
            {
                var insertNewUsage = new LicenseUsageReporting
                {
                    Day = 16,
                    LicensingIdentifier = licenseIdentifiers[i],
                    UsageCount = 3,
                    StartInterval = new DateTime(12, 12, 12, 5, 5, 5),
                    EndInterval = new DateTime(12, 12, 12, 5, 10, 5),
                    TenantId = Guid.NewGuid()
                };
                await memoryRepositoryFileLog.InsertAsync(insertNewUsage, true);
            }
            var expectedResult = new List<LicensesInUsageCountOutput>();
            expectedResult.Add(new LicensesInUsageCountOutput
            {
                UsageCount = await memoryRepositoryFileLog.Where(l => l.LicensingIdentifier == licenseIdentifiers[0]).SumAsync(l => l.UsageCount),
                EndInterval = (await memoryRepositoryFileLog.Where(l => l.LicensingIdentifier == licenseIdentifiers[0]).FirstAsync()).EndInterval,
                StartInterval = (await memoryRepositoryFileLog.Where(l => l.LicensingIdentifier == licenseIdentifiers[0]).FirstAsync()).StartInterval
            });
            var mockLicenseUsage = new Mock<ILicenseUsageService>();
            mockLicenseUsage.Setup(l => l.GetLicenseIdentifiersUsageForReporting("teste"))
                .ReturnsAsync(new List<Guid> {licenseIdentifiers[0]});
            var service = GetService(mockLicenseUsage);
            // execute
            var result = await service.CountLicensesInUsageByIdentifier("teste");
            // test
            result.Should().BeEquivalentTo(expectedResult);
        }
        
        [Fact(DisplayName = "Testa o retorno do método de licenças em uso (com dados salvos)")]
        public async Task Test_License_Usage_Returns_When_Have_Data()
        {
            // prepare data
            var memoryRepositoryFileLog = ServiceProvider.GetService<IRepository<LicenseUsageReporting>>();
            for (int i = 0; i < 5; i++)
            {
                var insertNewUsage = new LicenseUsageReporting
                {
                    Day = 16,
                    LicensingIdentifier = Guid.NewGuid(),
                    UsageCount = 3,
                    StartInterval = new DateTime(12, 12, 12, 5, 5, 5),
                    EndInterval = new DateTime(12, 12, 12, 5, 10, 5),
                    TenantId = Guid.NewGuid()
                };
                await memoryRepositoryFileLog.InsertAsync(insertNewUsage, true);
            }
            var expectedResult = new List<LicensesInUsageCountOutput>();
            expectedResult.Add(new LicensesInUsageCountOutput
            {
                UsageCount = await memoryRepositoryFileLog.SumAsync(l => l.UsageCount),
                EndInterval = (await memoryRepositoryFileLog.FirstAsync()).EndInterval,
                StartInterval = (await memoryRepositoryFileLog.FirstAsync()).StartInterval
            });
            var mockLicenseUsage = new Mock<ILicenseUsageService>();
            var service = GetService(mockLicenseUsage);
            // execute
            var result = await service.CountLicensesInUsageByIdentifier(null);
            // test
            result.Should().BeEquivalentTo(expectedResult);
        }

        private AnalyzeOnlineTenants GetService(Mock<ILicenseUsageService> licenseUsageService)
        {
            var mockApiCallBuilder = new Mock<IApiClientCallBuilder>();
            return ActivatorUtilities.CreateInstance<AnalyzeOnlineTenants>(ServiceProvider, mockApiCallBuilder.Object, licenseUsageService.Object);
        }
    }
}