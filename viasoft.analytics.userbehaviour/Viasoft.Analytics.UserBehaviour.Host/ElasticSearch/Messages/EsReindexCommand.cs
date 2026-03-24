using System;
using Viasoft.Core.ServiceBus.Abstractions;

namespace Viasoft.Analytics.UserBehaviour.Host.ElasticSearch.Messages
{
    [Endpoint("Viasoft.UserBehaviour.UsageHistoryIndex.Reindex", "Viasoft.UserBehaviour")]
    public class EsReindexCommand : ICommand, IInternalMessage
    {
        public Guid MessageIdentifier { get; set; }
        public int CurrentPage { get; set; }


        public EsReindexCommand()
        {
            MessageIdentifier = Guid.NewGuid();
            CurrentPage = 0;
        }
    }
}