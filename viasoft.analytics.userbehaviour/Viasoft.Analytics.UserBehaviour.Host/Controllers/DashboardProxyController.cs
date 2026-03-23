using Viasoft.Core.AmbientData.Abstractions;
using Viasoft.Core.Dashboard.Proxy;

namespace Viasoft.Analytics.UserBehaviour.Host.Controllers
{
    public class DashboardProxyController: Core.Dashboard.Proxy.DashboardProxyController
    {
        public DashboardProxyController(IDashboardProxyService dashboardProxyService, IAmbientDataCallOptionsResolver ambientDataCallOptionsResolver) : base(dashboardProxyService, ambientDataCallOptionsResolver)
        {
        }
    }
}