using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purchase.Application.DTO.Purchase
{
    public record PurchaseAirtimeRequestDTO
    {
        public Guid transactGuid { get; set; }
        public string merchandiseType { get; set; }

        public double transactAmount { get; set; }
        public string messageType { get; set; }

        public ProductID productID { get; set; }

        public DateTime transactDateTime { get; set; }
        public DateTime messageDateTime { get; set; }
        public TenderData tenderData { get; set; }

        public string merchantGroup { get; set; }
        public string merchantName { get; set; }
        public string comment { get; set; }
        public string storeId { get; set; }
        public string storeLocation { get; set; }
        public string terminalId { get; set; }
        public string operatorId { get; set; }
        public string basketId { get; set; }
        public string productData { get; set; }
        public string additionalData { get; set; }
        public string messageMode { get; set; }

        public Guid CorrelationId { get; set; }

        public Guid ParentId { get; set; }
        public string EventName { get; set; }
        public DateTime CreateDateTime { get; set; }


    }

    public record ProductID
    {
        public string MeterNumber { get; set; }
        public string MobileNetwork { get; set; }
        public string MobileNumber { get; set; }
        public string Barcode { get; set; }
        public string BillPaymentIdType { get; set; }
        public string BillPaymentIdValue { get; set; }
    }

    public record TenderData
    {
        public string type { get; set; }
        public decimal amount { get; set; }
        public string maskedPAN { get; set; }

        public string reference { get; set; }
    }

    
                                
    
}
