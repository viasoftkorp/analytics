using Viasoft.Core.ServiceBus.Abstractions;

namespace Viasoft.Analytics.UserBehaviour.Host.ElasticSearch.Messages
{
    [Endpoint("Viasoft.UserBehaviour.UsageHistoryIndex.Index", "Viasoft.UserBehaviour")]
    public class EsIndexCommand : ICommand, IInternalMessage
    {
    }
}