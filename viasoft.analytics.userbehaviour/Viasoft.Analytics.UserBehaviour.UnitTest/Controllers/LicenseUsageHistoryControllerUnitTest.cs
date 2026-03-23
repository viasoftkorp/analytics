using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Viasoft.Analytics.UserBehaviour.Domain.Enums;
using Viasoft.Analytics.UserBehaviour.Domain.LicenseUsageHistory.DTO;
using Viasoft.Analytics.UserBehaviour.Domain.LicenseUsageHistory.Service;
using Viasoft.Analytics.UserBehaviour.Host.Controllers;
using Viasoft.Analytics.UserBehaviour.Host.ElasticSearch.Services;
using Viasoft.Core.Testing.DateTimeProvider;
using Xunit;

namespace Viasoft.Analytics.UserBehaviour.Testing.Controllers
{
    public class LicenseUsageHistoryControllerUnitTest: AnalyticsUserBehaviourTestBase
    {
        [Fact(DisplayName = "Testa o GetAll")]
        public async Task Test_GetAll()
        {
            var input = new GetAllLicenseUsageHistory
            {
                SkipCount = 0,
                MaxResultCount = 20
            };
            var licenseUsageHistoryServiceMock = new Mock<ILicenseUsageHistoryService>();
            var indexServiceMock = new Mock<IEsUsageHistoryIndexService>();
            indexServiceMock
                .Setup(e => e.HasIndex())
                .Returns(Task.FromResult(true));
            var controller = GetController(licenseUsageHistoryServiceMock, indexServiceMock);
            await controller.GetAll(input);
            
            Assert.False(ServiceBus.FakeBus.Events.Any());
            Assert.Equal(1, licenseUsageHistoryServiceMock.Invocations.Count);
            Assert.Equal(nameof(ILicenseUsageHistoryService.GetAll), licenseUsageHistoryServiceMock.Invocations[0].Method.Name);
            Assert.Equal(input, licenseUsageHistoryServiceMock.Invocations[0].Arguments[0]);
        }

        [Fact(DisplayName = "Testa o GetAll sem Index")]
        public async Task Test_GetAll_No_Index()
        {
            var input = new GetAllLicenseUsageHistory
            {
                SkipCount = 0,
                MaxResultCount = 20
            };
            var licenseUsageHistoryServiceMock = new Mock<ILicenseUsageHistoryService>();
            var indexServiceMock = new Mock<IEsUsageHistoryIndexService>();
            indexServiceMock
                .Setup(e => e.HasIndex())
                .Returns(Task.FromResult(false));
            var controller = GetController(licenseUsageHistoryServiceMock, indexServiceMock);
            await controller.GetAll(input);
            
            Assert.True(ServiceBus.FakeBus.Events.Any());
            Assert.Equal(1, licenseUsageHistoryServiceMock.Invocations.Count);
            Assert.Equal(nameof(ILicenseUsageHistoryService.GetAll), licenseUsageHistoryServiceMock.Invocations[0].Method.Name);
            Assert.Equal(input, licenseUsageHistoryServiceMock.Invocations[0].Arguments[0]);
        }

        [Fact(DisplayName = "Testa o GetById")]
        public async Task Test_GetById()
        {
            var id = Guid.NewGuid();
            var licenseUsageHistoryServiceMock = new Mock<ILicenseUsageHistoryService>();
            var indexServiceMock = new Mock<IEsUsageHistoryIndexService>();
            var repo = ServiceProvider.GetService<Core.DDD.Repositories.IRepository<Domain.Entities.LicenseUsageHistory>>();
            await repo.InsertAsync(new Domain.Entities.LicenseUsageHistory
            {
                Id = id,
                Domain = Domains.Accounting,
                User = nameof(Domain.Entities.LicenseUsageHistory.User)
            }, true);
            var controller = GetController(licenseUsageHistoryServiceMock, indexServiceMock, repo);
            var result = await controller.GetById(id);
            Assert.NotNull(result);
        }

        private LicenseUsageHistoryController GetController(Mock<ILicenseUsageHistoryService> licenseUsageHistoryServiceMock, 
            Mock<IEsUsageHistoryIndexService> esUsageHistoryIndexServiceMock, Core.DDD.Repositories.IRepository<Domain.Entities.LicenseUsageHistory> repo = null)
        {
            repo ??= ServiceProvider.GetService<Core.DDD.Repositories.IRepository<Domain.Entities.LicenseUsageHistory>>();
            var mockDateTimeNow = new FakeDateTimeProvider(DateTime.Parse("2020-06-16 12:15:00"));
            var mapper = ServiceProvider.GetService<IMapper>();
                
            return new LicenseUsageHistoryController(repo, mapper, mockDateTimeNow, licenseUsageHistoryServiceMock.Object, 
                esUsageHistoryIndexServiceMock.Object, ServiceBus);
        }
    }
}