using EventBusServiceBus.EventDTOs;
using EventBusServiceBus.Events;
using MediatR;
using Microsoft.Extensions.Logging;
using Purchase.Application.ApplicationLogic;
using Purchase.Application.Commands;
using Purchase.Application.DTO.Purchase;
using Purchase.Application.Repositories.Interfaces;
using Purchase.Core.Events;
using Purchase.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Purchase.Application.EventHandlers
{
    
       public class AirtimePurchaseQueueingEventHandler : INotificationHandler<DomainEventNotification<AirtimePurchaseEvent<AirtimePurchaseDTO>>>
    {
        private readonly ILogger<AirtimePurchaseQueueingEventHandler> _logger;
        private readonly APIConnection _apiConnection;
        private readonly IAirtimePurchaseRepository _airtimePurchaseRepository;
        private readonly IMediator _mediator;
        public AirtimePurchaseQueueingEventHandler(
                                                    ILogger<AirtimePurchaseQueueingEventHandler> logger, 
                                                    APIConnection apiConnection,
                                                     IAirtimePurchaseRepository airtimePurchaseRepository,
                                                     IMediator mediator)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _apiConnection = apiConnection;
            _airtimePurchaseRepository = airtimePurchaseRepository;
            _mediator = mediator;

        }

        public async Task Handle(DomainEventNotification<AirtimePurchaseEvent<AirtimePurchaseDTO>> notification, CancellationToken cancellationToken)
        {
            /*
            notification.DomainEvent.IsPublished = true;

            _logger.LogInformation("Handled domain event. Event - {event}", notification.DomainEvent.GetType().Name);
            // Get requestPurchaseAirtimeRequestDTO object
            PurchaseAirtimeRequestDTO purchaseAirtimeRequestDTO = _airtimePurchaseRepository.createAirtimePurchaseRequest(notification.DomainEvent.airtimePurchaseEvent);
            // Send Post request to 
            HttpStatusCode httpStatusCode = await _apiConnection.PostResponse(purchaseAirtimeRequestDTO);

            if (httpStatusCode == HttpStatusCode.OK || httpStatusCode == HttpStatusCode.Created)
            {
                // Todo:  Create persisistence event for the data into database logic
                AirtimePurchaseDTO airtimePurchaseDTO = notification.DomainEvent.airtimePurchaseEvent;
               await _mediator.Send(new CreateMobileTransactionCommand(airtimePurchaseDTO));
                // Todo:  Create Successful event to publish topic purchase-success  to service Bus

                PurchaseSuccessPublisherIntegrationEvent purchaseSuccessPublisherIntegrationEvent = new PurchaseSuccessPublisherIntegrationEvent();
                //DomainEvent purchaseSuccessPublishEvent = new AirtimePurchaseEvent<PurchaseSuccessFailurePublishDTO>(new PurchaseSuccessFailurePublishDTO
                //{
                //    CorrelationId = Guid.NewGuid(),
                //    ParentId = notification.DomainEvent.airtimePurchaseEvent.CorrelationId,
                //    EventName = "PurchaseSuccessPublisher",
                //    CreateDateTime = DateTime.UtcNow
                //});
                await _mediator.Publish((INotification)Activator.CreateInstance(
                typeof(PurchaseSuccessPublisherIntegrationEventNotification<>).MakeGenericType(purchaseSuccessPublisherIntegrationEvent.GetType()), purchaseSuccessPublisherIntegrationEvent));
                

                _logger.LogInformation($"Successful Request");
            }
            else if (httpStatusCode == HttpStatusCode.BadRequest)
            {
                // Todo:  Create Failed event to publish topic  purchase-failure to service Bus
                VasPurchaseFailurePublisherIntegrationEvent vasPurchaseFailurePublisherIntegrationEvent = new VasPurchaseFailurePublisherIntegrationEvent();
                //DomainEvent purchaseFailurePublishEvent = new AirtimePurchaseEvent<PurchaseSuccessFailurePublishDTO>(new PurchaseSuccessFailurePublishDTO
                //{
                //    CorrelationId = Guid.NewGuid(),
                //    ParentId = notification.DomainEvent.airtimePurchaseEvent.CorrelationId,
                //    EventName = "VasPurchaseFailurePublisher",
                //    CreateDateTime = DateTime.UtcNow
                //});
                await _mediator.Publish((INotification)Activator.CreateInstance(
                typeof(VasPurchaseFailurePublisherIntegrationEventNotification<>).MakeGenericType(vasPurchaseFailurePublisherIntegrationEvent.GetType()), vasPurchaseFailurePublisherIntegrationEvent));
                _logger.LogInformation($"Unsuccessful Request");
            }
            else if (httpStatusCode == HttpStatusCode.NotFound)
            {
                // Todo: Event to check if the link is up
                _logger.LogCritical($"Url:Service is down");
            }
            else
            {
                _logger.LogError("unexpected error");
            }
            */
            

        }

        private INotification GetNotificationCorrespondingTIntegrationEvent(IntegrationEvent integrationEvent)
        {
            return (INotification)Activator.CreateInstance(
                typeof(VasPurchaseFailurePublisherIntegrationEventNotification<>).MakeGenericType(integrationEvent.GetType()), integrationEvent);
        }
    }
}
