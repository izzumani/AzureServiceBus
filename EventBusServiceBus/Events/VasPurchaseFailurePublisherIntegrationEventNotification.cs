using MediatR;
using Purchase.Core.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Purchase.Core.Events
{
    public class VasPurchaseFailurePublisherIntegrationEventNotification<VasPurchaseFailurePublisherIntegrationEvent> : INotification
    
    {
        public VasPurchaseFailurePublisherIntegrationEventNotification(VasPurchaseFailurePublisherIntegrationEvent integrationEvent)
        {
            IntegrationEvent = integrationEvent;
        }

        public VasPurchaseFailurePublisherIntegrationEvent IntegrationEvent { get; }
    }
}
