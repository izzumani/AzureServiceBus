using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purchase.Application.DTO.Purchase
{
    public class PurchaseSuccessFailurePublishDTO
    {
        public Guid CorrelationId {get; set;}
        
        public Guid ParentId { get; set; }

        public string EventName { get; set; }

        public DateTime CreateDateTime { get; set; }
                       
    }
}
