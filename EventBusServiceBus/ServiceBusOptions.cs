#nullable enable

namespace EventBusServiceBus
{
    public record ServiceBusOptions(

        string EventBusConnection,
        string? SubscriptionClientName,
        string? TopicName,
        string? QueueName,
        int ReceiveMode,
        int PrefetchCount,
        bool AutoCompleteMessages,
        TimeSpan MaxAutoLockRenewalDuration,
        TimeSpan MaxReceiveWaitTime,
        int MaxConcurrentCalls,
        SubQueue SubQueue);
}
