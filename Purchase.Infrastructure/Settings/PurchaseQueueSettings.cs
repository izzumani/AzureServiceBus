namespace Purchase.Infrastructure.Settings
{
    public class PurchaseQueueSettings : ServiceBusSettings
    {
        public string? QueueName { get; set; }
    }
}
