using System.Linq;
using System.Threading.Tasks;
using Moq;
using Viasoft.Analytics.UserBehaviour.Domain.LicenseUsageHistory.DTO;
using Viasoft.Analytics.UserBehaviour.Domain.LicenseUsageHistory.Enum;
using Viasoft.Analytics.UserBehaviour.Domain.LicenseUsageHistory.Service;
using Viasoft.Analytics.UserBehaviour.Host.Controllers;
using Viasoft.Analytics.UserBehaviour.Host.ElasticSearch.Services;
using Viasoft.Core.Testing;
using Xunit;

namespace Viasoft.Analytics.UserBehaviour.Testing.Controllers
{
    public class UsageSearchControllerUnitTest: AnalyticsUserBehaviourTestBase
    {
        [Fact(DisplayName = "Testa o FilterByGrouping")]
        public async Task Test_FilterByGrouping()
        {
            var input = new FilterByGroupingInput
            {
                Groupings = Groupings.App
            };
            var filterByGroupingServiceMock = new Mock<IFilterByGroupingService>();
            var indexServiceMock = new Mock<IEsUsageHistoryIndexService>();
            indexServiceMock
                .Setup(e => e.HasIndex())
                .Returns(Task.FromResult(true));
            var controller = GetController(filterByGroupingServiceMock, indexServiceMock);
            await controller.FilterByGrouping(input);
            
            Assert.False(ServiceBus.FakeBus.Events.Any());
            Assert.Equal(1, filterByGroupingServiceMock.Invocations.Count);
            Assert.Equal(nameof(IFilterByGroupingService.FilterByGrouping), filterByGroupingServiceMock.Invocations[0].Method.Name);
            Assert.Equal(input, filterByGroupingServiceMock.Invocations[0].Arguments[0]);
        }
        
        [Fact(DisplayName = "Testa o FilterByGrouping sem Index")]
        public async Task Test_FilterByGrouping_No_Index()
        {
            var input = new FilterByGroupingInput
            {
                Groupings = Groupings.App
            };
            var filterByGroupingServiceMock = new Mock<IFilterByGroupingService>();
            var indexServiceMock = new Mock<IEsUsageHistoryIndexService>();
            indexServiceMock
                .Setup(e => e.HasIndex())
                .Returns(Task.FromResult(false));
            var controller = GetController(filterByGroupingServiceMock, indexServiceMock);
            await controller.FilterByGrouping(input);
            
            Assert.True(ServiceBus.FakeBus.Events.Any());
            Assert.Equal(1, filterByGroupingServiceMock.Invocations.Count);
            Assert.Equal(nameof(IFilterByGroupingService.FilterByGrouping), filterByGroupingServiceMock.Invocations[0].Method.Name);
            Assert.Equal(input, filterByGroupingServiceMock.Invocations[0].Arguments[0]);
        }

        private UsageSearchController GetController(Mock<IFilterByGroupingService> filterByGroupingServiceMock, Mock<IEsUsageHistoryIndexService> esUsageHistoryIndexServiceMock)
        {
            return new UsageSearchController(filterByGroupingServiceMock.Object, esUsageHistoryIndexServiceMock.Object, ServiceBus);
        }
    }
}