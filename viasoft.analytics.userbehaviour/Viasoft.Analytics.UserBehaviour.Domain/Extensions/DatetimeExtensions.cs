using System;
using NodaTime;
using NodaTime.Extensions;

namespace Viasoft.Analytics.UserBehaviour.Domain.Extensions
{
    public static class DatetimeExtensions
    {
        public static DateTime WithAppliedOffset(this DateTime utcDateTime, string timeZoneId)
        {
            var timeZone = DateTimeZoneProviders.Tzdb.GetZoneOrNull(timeZoneId);
            var offset = timeZone.GetUtcOffset(utcDateTime.ToInstant()).ToTimeSpan();
            var date = utcDateTime.Add(-offset);
            return date;
        }          
        public static DateTime RemovingOffset(this DateTime utcDateTime, string timeZoneId)
        {
            var timeZone = DateTimeZoneProviders.Tzdb.GetZoneOrNull(timeZoneId);
            var offset = timeZone.GetUtcOffset(utcDateTime.ToInstant()).ToTimeSpan();
            var date = utcDateTime.Add(offset);
            return date;
        }       
        public static DateTime AsUtc(this DateTime dateTime)
        {
            return DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
        }

        public static DateTime GetStartOfDay(this DateTime dateTime, string timeZone)
        {
            var correctedDatetime = dateTime.RemovingOffset(timeZone);
            return new DateTime(correctedDatetime.Year, correctedDatetime.Month, correctedDatetime.Day).AsUtc().WithAppliedOffset(timeZone);
        }        
        public static DateTime GetStartOfMonth(this DateTime utcDateTime, string timeZoneId)
        {
            var correctedDatetime = utcDateTime.RemovingOffset(timeZoneId);
            var timeZonedDateTime = new DateTime(correctedDatetime.Year, correctedDatetime.Month, 1).AsUtc().WithAppliedOffset(timeZoneId);
            return timeZonedDateTime;
        }         
        public static DateTime GetEndOfMonth(this DateTime utcDateTime, string timeZoneId)
        {
            var correctedDatetime = utcDateTime.RemovingOffset(timeZoneId);
            var dateTime = new DateTime(correctedDatetime.Year, correctedDatetime.Month, 1).AddMonths(1).AsUtc();
            dateTime = dateTime.AddTicks(-1).WithAppliedOffset(timeZoneId);
            return dateTime;
        }        
    }
}