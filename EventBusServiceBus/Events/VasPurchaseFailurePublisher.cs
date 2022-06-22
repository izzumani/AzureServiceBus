using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBusServiceBus.Events
{
    public record VasPurchaseFailurePublisherIntegrationEvent : IntegrationEvent
    {
        public VasPurchaseFailurePublisherIntegrationEvent() : base()
        {
            ParentId = base.Id;
            CorrelationId = Guid.NewGuid();
            EventName = "VasPurchaseFailurePublisher";

        }

        [JsonInclude]
        public Guid CorrelationId { get; private init; }

        [JsonInclude]
        public Guid ParentId { get; private init; }

        [JsonInclude]
        public string EventName { get; private init; }

    }
}
