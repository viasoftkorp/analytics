using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Viasoft.Analytics.UserBehaviour.Domain.DTOs.LicenseUsageHistoryIndex;
using Viasoft.Analytics.UserBehaviour.Host.ElasticSearch.Messages;
using Viasoft.Analytics.UserBehaviour.Host.ElasticSearch.Services;
using Viasoft.Core.AspNetCore.Controller;
using Viasoft.Core.ServiceBus.Abstractions;

namespace Viasoft.Analytics.UserBehaviour.Host.Controllers
{
    public class LicenseUsageHistoryIndexController: BaseController
    {
        private readonly IEsUsageHistoryIndexService _esUsageHistoryIndexService;
        private readonly IMapper _mapper;
        private readonly IServiceBus _serviceBus;

        public LicenseUsageHistoryIndexController(IEsUsageHistoryIndexService esUsageHistoryIndexService, IMapper mapper, IServiceBus serviceBus)
        {
            _esUsageHistoryIndexService = esUsageHistoryIndexService;
            _mapper = mapper;
            _serviceBus = serviceBus;
        }

        [HttpGet]
        public async Task<LicenseUsageHistoryIndexOutput> Get()
        {
            var index = await _esUsageHistoryIndexService.GetHistoryIndex();
            return _mapper.Map<LicenseUsageHistoryIndexOutput>(index);
        }
        
        [HttpPost]
        public async Task Reindex()
        {
            await _serviceBus.SendLocal(new EsReindexCommand());
        }
    }
}