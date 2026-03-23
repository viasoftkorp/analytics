using System;

namespace Viasoft.Analytics.UserBehaviour.Domain.LicenseUsageHistory.DTO
{
    public class IntervalDto
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public IntervalDto(DateTime startDate, DateTime endDate)
        {
            StartDate = startDate;
            EndDate = endDate;
        }
    }
}