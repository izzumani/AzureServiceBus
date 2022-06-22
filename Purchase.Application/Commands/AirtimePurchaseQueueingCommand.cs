using EventBusServiceBus.EventDTOs;
using MediatR;
using Microsoft.Extensions.Logging;
using Purchase.Application.DTO.Purchase;
using Purchase.Core.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purchase.Application.Commands
{
    public class AirtimePurchaseQueueingCommand : IRequest<string>
    {
        public AirtimePurchaseDTO _airtimePurchaseDTO { get; }
        public AirtimePurchaseQueueingCommand(AirtimePurchaseDTO airtimePurchaseDTO)
        {
            _airtimePurchaseDTO = airtimePurchaseDTO;
        }
    }
        public class AirtimePurchaseQueueingCommandHandler : IRequestHandler<AirtimePurchaseQueueingCommand, string>
        {
        private readonly IMediator _mediator;
        private readonly ILogger<AirtimePurchaseQueueingCommandHandler> _logger;
        public AirtimePurchaseQueueingCommandHandler(IMediator mediator, 
                                                    ILogger<AirtimePurchaseQueueingCommandHandler> logger)
            {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

            public async Task<string> Handle(AirtimePurchaseQueueingCommand request, CancellationToken cancellationToken)
            {
            
            DomainEvent airtimePurchaseEvent = new AirtimePurchaseEvent<AirtimePurchaseDTO>(request._airtimePurchaseDTO);

            _logger.LogInformation("Publishing domain event. Event - {event}", airtimePurchaseEvent.GetType().Name);

            await _mediator.Publish(GetNotificationCorrespondingToDomainEvent(airtimePurchaseEvent));
            await Task.Delay(1000); //Note: Just to be sure the event handler received the request.
            if (airtimePurchaseEvent.IsPublished==true)
            {
                _logger.LogInformation($"Domain event. Event - {airtimePurchaseEvent.GetType().Name} Successfully");
                return "Successfully";
            }
            else
            {
                _logger.LogInformation($"Domain event. Event - {airtimePurchaseEvent.GetType().Name} Failed");
                return "Failed";
            }
            

            }

        private INotification GetNotificationCorrespondingToDomainEvent(DomainEvent domainEvent)
        {
            return (INotification)Activator.CreateInstance(
                typeof(DomainEventNotification<>).MakeGenericType(domainEvent.GetType()), domainEvent);
        }
    }


    
}
