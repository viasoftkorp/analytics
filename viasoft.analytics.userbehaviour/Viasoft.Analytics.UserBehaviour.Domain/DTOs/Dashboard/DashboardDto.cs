using System;

namespace Viasoft.Analytics.UserBehaviour.Domain.DTOs.Dashboard
{
    public class DashboardDto
    {
        public Guid ConsumerId {get; set;}
        public byte[] SerializedDashboard {get; set;}
    }
}