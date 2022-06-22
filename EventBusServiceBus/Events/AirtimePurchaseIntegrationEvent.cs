using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBusServiceBus.Events
{
    public record AirtimePurchaseIntegrationEvent<AirtimePurchaseDTO> : IntegrationEvent
    {
        public AirtimePurchaseIntegrationEvent(AirtimePurchaseDTO airtimePurchaseDTO) :base(){
            _airtimePurchaseDTO = airtimePurchaseDTO;
        }

        public AirtimePurchaseDTO _airtimePurchaseDTO;
    }
}
