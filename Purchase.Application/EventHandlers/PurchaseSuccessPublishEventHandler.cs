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
    public record PurchaseSuccessPublishEventHandler : INotificationHandler<PurchaseSuccessPublisherIntegrationEventNotification<PurchaseSuccessPublisherIntegrationEvent>>
    {
        private readonly ILogger<PurchaseSuccessPublishEventHandler> _logger;
        private readonly IEventBus _eventBus;

        public PurchaseSuccessPublishEventHandler( ILogger<PurchaseSuccessPublishEventHandler> logger, IEventBus eventBus)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _eventBus = eventBus;
        }

        public async Task Handle(PurchaseSuccessPublisherIntegrationEventNotification<PurchaseSuccessPublisherIntegrationEvent> notification, CancellationToken cancellationToken)
        {
            _eventBus.Publish(notification.IntegrationEvent);
            _logger.LogDebug("Logged Intergration Event Notification for PurchaseSuccessPublisher");

            //Todo: Publish to Azure Service Bus


        }
    }

}
