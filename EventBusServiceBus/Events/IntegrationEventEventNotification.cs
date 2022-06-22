using EventBusServiceBus.EventDTOs;
using MediatR;
using Purchase.Core.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace EventBusServiceBus.Events
{
    public record IntegrationEventEventNotification<TIntegrationEventvent> : INotification where TIntegrationEventvent : AirtimePurchaseIntegrationEvent<AirtimePurchaseDTO>

    {
        public IntegrationEventEventNotification(AirtimePurchaseIntegrationEvent<AirtimePurchaseDTO> domainEvent)
        {
            DomainEvent = domainEvent;
        }

        public AirtimePurchaseIntegrationEvent<AirtimePurchaseDTO> DomainEvent { get; }
    }
}
