namespace EventBusServiceBus.EventDTOs
{
    public class AirtimePurchaseDTO
    {
        public Guid TransactionId { get; set; }
        public string MobileNumber { get; set; }
        public string MobileNetwork { get; set; }
        public string TraderId { get; set; }
        public double TransactionAmount { get; set; }
        public int ProductId { get; set; }
        public Guid CorrelationId { get; set; }
        public Guid? ParentId { get; set; }
        public DateTime CreatedDateTime { get; set; }
    }
}
