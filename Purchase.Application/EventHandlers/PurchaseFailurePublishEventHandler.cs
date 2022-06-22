using EventBusServiceBus.Abstractions;
using EventBusServiceBus.Events;
using MediatR;
using Microsoft.Extensions.Logging;
using Purchase.Application.DTO.Purchase;
using Purchase.Core.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purchase.Application.EventHandlers
{
    public record PurchaseFailurePublishEventHandler : INotificationHandler<VasPurchaseFailurePublisherIntegrationEventNotification<VasPurchaseFailurePublisherIntegrationEvent>>
    {
        private readonly ILogger<PurchaseFailurePublishEventHandler> _logger;
        private readonly IEventBus _eventBus;

        public PurchaseFailurePublishEventHandler( ILogger<PurchaseFailurePublishEventHandler> logger, IEventBus eventBus)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _eventBus = eventBus;
        }

        public async Task Handle(VasPurchaseFailurePublisherIntegrationEventNotification<VasPurchaseFailurePublisherIntegrationEvent> notification, CancellationToken cancellationToken)
        {
            _eventBus.Publish(notification.IntegrationEvent);
            _logger.LogDebug("Logged Intergration Event Notification for VasPurchaseFailurePublisher");

            //Todo: Publish to Azure Service Bus


        }
    }

}
