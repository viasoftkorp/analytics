using System;
using System.Threading.Tasks;
using Viasoft.Analytics.UserBehaviour.Domain.Extensions;
using Viasoft.Analytics.UserBehaviour.Domain.LicenseUsageHistory.DTO;
using Viasoft.Core.DateTimeProvider;
using Viasoft.Core.Identity.Abstractions;
using Viasoft.Core.Identity.Abstractions.Store;
using Viasoft.Core.IoC.Abstractions;

namespace Viasoft.Analytics.UserBehaviour.Domain.Services.DateIntervalProviders
{
    public class DateIntervalProvider : IDateIntervalProvider, ITransientDependency
    {
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly ICurrentUser _currentUser;
        private readonly IUserStore _userStore;

        public DateIntervalProvider(ICurrentUser currentUser, IDateTimeProvider dateTimeProvider,IUserStore userStore)
        {
            _currentUser = currentUser;
            _dateTimeProvider = dateTimeProvider;
            _userStore = userStore;
        }
        public async Task<IntervalDto> GetInterval(DateInterval rangeType)
        {
            return rangeType switch
            {
                DateInterval.AllTimes => null,
                DateInterval.Today => await Today(),
                DateInterval.Last7Days => await LastXDays(7),
                DateInterval.Last30Days => await LastXDays(30),
                DateInterval.Last90Days => await LastXDays(90),
                DateInterval.Last6Months => await LastXMonths(6),
                DateInterval.Last12Months => await LastXMonths(12),
                _ => throw new ArgumentOutOfRangeException(nameof(rangeType), rangeType, null)
            };
        }
        private async Task<string> UserTimezone()
        {
            var userPreferences =  await _userStore.GetUserPreferencesAsync(_currentUser.Id);

            return userPreferences.DefaultUserTimeZone;
        }
        private async Task<IntervalDto> Today()
        {
            var userTimezone = await UserTimezone() ;
            var startDate = _dateTimeProvider.UtcNow().GetStartOfDay(userTimezone);
            var endDate = _dateTimeProvider.UtcNow().WithAppliedOffset(userTimezone);
            var interval = new IntervalDto(startDate , endDate);
            return interval;
        } 
        private async Task<IntervalDto> LastXMonths(int months)
        {
            var userPreferences =  await _userStore.GetUserPreferencesAsync(_currentUser.Id);
            var userTimezone = userPreferences.DefaultUserTimeZone;
            var startDate = _dateTimeProvider.UtcNow().AddMonths(-(months-1)).GetStartOfMonth(userTimezone);
            var endDate = _dateTimeProvider.UtcNow().GetEndOfMonth(userTimezone);
            var interval = new IntervalDto(startDate , endDate);
            return interval;
        }       
        private async Task<IntervalDto> LastXDays(int days)
        {
            var userPreferences =  await _userStore.GetUserPreferencesAsync(_currentUser.Id);
            var userTimezone = userPreferences.DefaultUserTimeZone;
            var startDate = _dateTimeProvider.UtcNow().AddDays(-(days-1)).GetStartOfDay(userTimezone);
            var endDate = _dateTimeProvider.UtcNow().AsUtc().WithAppliedOffset(userTimezone);
            var interval = new IntervalDto(startDate , endDate);
            return interval;
        }
    }
}