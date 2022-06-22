using MediatR;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace EventBusServiceBus.Events
{
    public record PurchaseSuccessPublisherIntegrationEventNotification<PurchaseSuccessPublisherIntegrationEvent> : INotification
    {
        public PurchaseSuccessPublisherIntegrationEventNotification(PurchaseSuccessPublisherIntegrationEvent integrationEvent)
        {
        IntegrationEvent = integrationEvent;
        }

        public PurchaseSuccessPublisherIntegrationEvent IntegrationEvent { get; }
    }
}
