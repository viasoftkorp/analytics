using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Viasoft.Analytics.UserBehaviour.Domain.DTOs.LicenseUsageStatistics;
using Viasoft.Analytics.UserBehaviour.Domain.Services.AnalyzeOnlineTenantsService;
using Viasoft.Analytics.UserBehaviour.Host.Controllers;
using Viasoft.Core.Testing;
using Xunit;

namespace Viasoft.Analytics.UserBehaviour.Testing.Controllers
{
    public class UserBehaviourAnalyticsControllerUnitTest: AnalyticsUserBehaviourTestBase
    {
        [Fact(DisplayName = "Testa se o metodo para buscar as licenças em uso esta chamando corretamente o serviço")]
        public async Task Test_Analytics_Call_For_Usage_License()
        {
            // prepare data
            var mockAnalyze = new Mock<IAnalyzeOnlineTenants>();
            mockAnalyze.Setup(m => m.CountLicensesInUsageByIdentifier("teste"))
                .ReturnsAsync(new List<LicensesInUsageCountOutput>());
            var controller = GetController(mockAnalyze);
            // execute
            await controller.CountLicensesInUsageByIdentifier("teste");
            // test
            Assert.Equal(1, mockAnalyze.Invocations.Count);
            mockAnalyze.Invocations[0].Arguments[0].Should().BeEquivalentTo("teste");
        }

        private UserBehaviourAnalyticsController GetController(Mock<IAnalyzeOnlineTenants> analyzeMock)
        {
            return ActivatorUtilities.CreateInstance<UserBehaviourAnalyticsController>(ServiceProvider,
                analyzeMock.Object);
        }
    }
}