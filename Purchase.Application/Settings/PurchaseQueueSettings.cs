namespace Purchase.Application.Settings
{
    public class PurchaseQueueSettings : ServiceBusSettings
    {
        public string? QueueName { get; set; }
    }
}
