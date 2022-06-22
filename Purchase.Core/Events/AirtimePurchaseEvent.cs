using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Purchase.Core.Events
{
    public class AirtimePurchaseEvent<T> : DomainEvent
    {
        public T airtimePurchaseEvent { get; }

        public AirtimePurchaseEvent(T _airtimePurchaseEvent)
        {
            airtimePurchaseEvent = _airtimePurchaseEvent;
        }

    }
}
