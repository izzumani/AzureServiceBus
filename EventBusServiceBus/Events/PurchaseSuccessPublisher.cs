using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBusServiceBus.Events
{
    public record PurchaseSuccessPublisherIntegrationEvent : IntegrationEvent
    {
        public PurchaseSuccessPublisherIntegrationEvent() : base() 
        {
            ParentId = base.Id;
            CorrelationId = Guid.NewGuid();
            EventName = "PurchaseSuccessPublisher";

        }

        [JsonInclude]
        public Guid CorrelationId { get; private init; }

        [JsonInclude]
        public Guid ParentId { get; private init; }

        [JsonInclude]
        public string EventName { get; private init; }
        
        
    }

}
