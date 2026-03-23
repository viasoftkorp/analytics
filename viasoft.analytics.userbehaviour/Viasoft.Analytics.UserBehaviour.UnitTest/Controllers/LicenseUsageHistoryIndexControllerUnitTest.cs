using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using AutoMapper;
using Moq;
using Viasoft.Analytics.UserBehaviour.Domain.Entities;
using Viasoft.Analytics.UserBehaviour.Host.Controllers;
using Viasoft.Analytics.UserBehaviour.Host.ElasticSearch.Services;
using Xunit;

namespace Viasoft.Analytics.UserBehaviour.Testing.Controllers
{
    public class LicenseUsageHistoryIndexControllerUnitTest: AnalyticsUserBehaviourTestBase
    {
        [Fact(DisplayName = "Testa o Get")]
        public async Task Test_Get()
        {
            var index = new LicenseUsageHistoryIndex
            {
                Id = Guid.NewGuid(),
                HasEsFailedSinceLastReindex = true,
                LastModificationTime = DateTime.UtcNow
            };
            var indexServiceMock = new Mock<IEsUsageHistoryIndexService>();
            indexServiceMock
                .Setup(e => e.GetHistoryIndex(null))
                .Returns(Task.FromResult(index));
            var controller = GetController(indexServiceMock);
            var result = await controller.Get();
            Assert.True(result.HasEsFailedSinceLastReindex);
            Assert.False(result.IsIndexing);
            Assert.Equal(index.LastModificationTime, result.LastModificationTime);
            
            Assert.Equal(1, indexServiceMock.Invocations.Count);
            Assert.Equal(nameof(IEsUsageHistoryIndexService.GetHistoryIndex), indexServiceMock.Invocations[0].Method.Name);
        }

        [Fact(DisplayName = "Testa o Reindex")]
        public async Task Test_Reindex()
        {
            var index = new LicenseUsageHistoryIndex
            {
                Id = Guid.NewGuid(),
                HasEsFailedSinceLastReindex = true,
                LastModificationTime = DateTime.UtcNow
            };
            var indexServiceMock = new Mock<IEsUsageHistoryIndexService>();
            indexServiceMock
                .Setup(e => e.GetHistoryIndex(null))
                .Returns(Task.FromResult(index));
            var controller = GetController(indexServiceMock);
            await controller.Reindex();
            
            Assert.True(ServiceBus.FakeBus.Events.Any());
        }

        private LicenseUsageHistoryIndexController GetController(Mock<IEsUsageHistoryIndexService> esUsageHistoryIndexServiceMock)
        {
            var mapper = ServiceProvider.GetService<IMapper>();
            
            return new LicenseUsageHistoryIndexController(esUsageHistoryIndexServiceMock.Object, mapper, ServiceBus);
        }
    }
}