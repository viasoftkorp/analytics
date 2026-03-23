using System.Threading.Tasks;
using Rebus.Handlers;
using Viasoft.Analytics.UserBehaviour.Host.ElasticSearch.Messages;
using Viasoft.Analytics.UserBehaviour.Host.ElasticSearch.Services;

namespace Viasoft.Analytics.UserBehaviour.Host.Handlers
{
    public class UsageHistoryIndexHandler: IHandleMessages<EsIndexMessage>, IHandleMessages<EsReindexMessage>
    {
        private readonly IEsUsageHistoryIndexService _esUsageHistoryIndexService;

        public UsageHistoryIndexHandler(IEsUsageHistoryIndexService esUsageHistoryIndexService)
        {
            _esUsageHistoryIndexService = esUsageHistoryIndexService;
        }

        public async Task Handle(EsIndexMessage message)
        {
            await _esUsageHistoryIndexService.SeedHistories();
        }

        public async Task Handle(EsReindexMessage message)
        {
            await _esUsageHistoryIndexService.Reindex(message.MessageIdentifier, message.CurrentPage);
        }
    }
}