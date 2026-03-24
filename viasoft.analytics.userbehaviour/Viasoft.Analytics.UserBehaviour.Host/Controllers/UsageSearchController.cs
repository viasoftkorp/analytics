using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Viasoft.Analytics.UserBehaviour.Domain.LicenseUsageHistory.DTO;
using Viasoft.Analytics.UserBehaviour.Domain.LicenseUsageHistory.Service;
using Viasoft.Analytics.UserBehaviour.Host.ElasticSearch.Messages;
using Viasoft.Analytics.UserBehaviour.Host.ElasticSearch.Services;
using Viasoft.Core.AspNetCore.Controller;
using Viasoft.Core.ServiceBus.Abstractions;

namespace Viasoft.Analytics.UserBehaviour.Host.Controllers
{
    public class UsageSearchController : BaseController
    {
        private readonly IFilterByGroupingService _filterByGroupingService;
        private readonly IEsUsageHistoryIndexService _historyIndexService;
        private readonly IServiceBus _serviceBus;

        public UsageSearchController(IFilterByGroupingService filterByGroupingService, IEsUsageHistoryIndexService historyIndexService, IServiceBus serviceBus)
        {
            _filterByGroupingService = filterByGroupingService;
            _historyIndexService = historyIndexService;
            _serviceBus = serviceBus;
        }

        [HttpGet]
        public async Task<FilterByGroupingOutput> FilterByGrouping([FromQuery] FilterByGroupingInput input)
        {
            var hasIndex = await _historyIndexService.HasIndex();
            if (!hasIndex)
            {
                await _serviceBus.SendLocal(new EsIndexCommand());
            }
            return await _filterByGroupingService.FilterByGrouping(input);
        }
    }
}