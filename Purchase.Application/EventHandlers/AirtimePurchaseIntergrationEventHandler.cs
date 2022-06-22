using EventBusServiceBus.Abstractions;
using EventBusServiceBus.EventDTOs;
using EventBusServiceBus.Events;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Purchase.Application.ApplicationLogic;
using Purchase.Application.Commands;
using Purchase.Application.DTO.Purchase;
using Purchase.Application.Repositories.Interfaces;
using Purchase.Core.Events;
using Purchase.Infrastructure.Services;
using System.Net;

namespace Purchase.Application.EventHandlers
{
    
    public record AirtimePurchaseIntergrationEventHandler : INotificationHandler<IntegrationEventEventNotification<AirtimePurchaseIntegrationEvent<AirtimePurchaseDTO>>>, IIntegrationEventHandler, IDynamicIntegrationEventHandler
    {
        private readonly IMediator _mediator;
        private readonly ILogger<AirtimePurchaseIntergrationEventHandler> _logger;
        private readonly APIConnection _apiConnection;
        private readonly IAirtimePurchaseRepository _airtimePurchaseRepository;
        public AirtimePurchaseIntergrationEventHandler (
             ILogger<AirtimePurchaseIntergrationEventHandler> logger,
               APIConnection apiConnection,
                IAirtimePurchaseRepository airtimePurchaseRepository,
                IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
            _apiConnection = apiConnection;
            _airtimePurchaseRepository = airtimePurchaseRepository;
        }
        public async Task Handle(IntegrationEventEventNotification<AirtimePurchaseIntegrationEvent<AirtimePurchaseDTO>> @event, CancellationToken cancellationToken)
        
        {
            _logger.LogDebug("Processing of request expected to reach here from Subscribe");

            

            _logger.LogInformation("Handled domain event. Event - {event}", @event.GetType().Name);
            // Get requestPurchaseAirtimeRequestDTO object
            PurchaseAirtimeRequestDTO purchaseAirtimeRequestDTO = _airtimePurchaseRepository.createAirtimePurchaseRequest(@event.DomainEvent._airtimePurchaseDTO);
            // Send Post request to 
            HttpStatusCode httpStatusCode = await _apiConnection.PostResponse(purchaseAirtimeRequestDTO);

            if (httpStatusCode == HttpStatusCode.OK || httpStatusCode == HttpStatusCode.Created)
            {
                // Todo:  Create persisistence event for the data into database logic
                AirtimePurchaseDTO airtimePurchaseDTO = @event.DomainEvent._airtimePurchaseDTO;
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

        }

        public async Task Handle(dynamic eventData)
        {
            _logger.LogInformation("Handling dynamically");
        }
    }
}
