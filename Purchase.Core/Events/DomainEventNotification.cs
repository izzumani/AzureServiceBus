using MediatR;
using Purchase.Core.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Purchase.Core.Events
{
    public class DomainEventNotification<TDomainEvent> : INotification where TDomainEvent : DomainEvent
    
    {
        public DomainEventNotification(TDomainEvent domainEvent)
        {
            DomainEvent = domainEvent;
        }

        public TDomainEvent DomainEvent { get; }
    }
}
