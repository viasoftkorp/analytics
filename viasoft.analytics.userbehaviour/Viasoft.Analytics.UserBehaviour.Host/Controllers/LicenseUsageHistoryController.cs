using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Viasoft.Analytics.UserBehaviour.Domain.Entities;
using Viasoft.Analytics.UserBehaviour.Domain.LicenseUsageHistory.DTO;
using Viasoft.Analytics.UserBehaviour.Domain.LicenseUsageHistory.Service;
using Viasoft.Analytics.UserBehaviour.Host.ElasticSearch.Messages;
using Viasoft.Analytics.UserBehaviour.Host.ElasticSearch.Services;
using Viasoft.Core.AspNetCore.Controller;
using Viasoft.Core.DateTimeProvider;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Core.DDD.Repositories;
using Viasoft.Core.ServiceBus.Abstractions;

namespace Viasoft.Analytics.UserBehaviour.Host.Controllers
{
    public class LicenseUsageHistoryController: BaseReadonlyController<LicenseUsageHistory, LicenseUsageHistoryOutput, GetAllLicenseUsageHistory, string>
    {
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly ILicenseUsageHistoryService _licenseUsageHistoryService;
        private readonly IEsUsageHistoryIndexService _historyIndexService;
        private readonly IServiceBus _serviceBus;

        public LicenseUsageHistoryController(IReadOnlyRepository<LicenseUsageHistory> repository, IMapper mapper, IDateTimeProvider dateTimeProvider, ILicenseUsageHistoryService licenseUsageHistoryService, IEsUsageHistoryIndexService historyIndexService, IServiceBus serviceBus) : base(repository, mapper)
        {
            _dateTimeProvider = dateTimeProvider;
            _licenseUsageHistoryService = licenseUsageHistoryService;
            _historyIndexService = historyIndexService;
            _serviceBus = serviceBus;
        }

        public override async Task<PagedResultDto<LicenseUsageHistoryOutput>> GetAll(GetAllLicenseUsageHistory input)
        {
            var hasIndex = await _historyIndexService.HasIndex();
            if (!hasIndex)
            {
                await _serviceBus.SendLocal(new EsIndexCommand());
            }
            return await _licenseUsageHistoryService.GetAll(input);
        }

        public override async Task<LicenseUsageHistoryOutput> GetById(Guid id)
        {
            var entity = await Repository.FindAsync(id);
                
            var output = Mapper.Map<LicenseUsageHistoryOutput>(entity);

            output.AccessDuration = entity.AccessDuration(_dateTimeProvider);
            output.AccessDurationInMinutes = entity.AccessDurationInMinutes(_dateTimeProvider);

            return output;
        }

        protected override (Expression<Func<LicenseUsageHistory, string>>, bool) DefaultGetAllSorting()
        {
            return (l => l.User, true);
        }
    }
}