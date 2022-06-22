using Azure.Messaging.ServiceBus;

namespace Purchase.Application.Settings
{
    public class ServiceBusSettings
    {
        public string EventBusConnection { get; set; }
        public int ReceiveMode { get; set; }
        public int PrefetchCount { get; set; }
        public bool AutoCompleteMessages { get; set; }
        public TimeSpan MaxAutoLockRenewalDuration { get; set; }
        public TimeSpan MaxReceiveWaitTime { get; set; }
        public int MaxConcurrentCalls { get; set; }
        public SubQueue SubQueue { get; set; }
    }
}
