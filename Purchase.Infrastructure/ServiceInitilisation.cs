using EventBusServiceBus.Events;
using MediatR;
using Purchase.Infrastructure.Settings;

namespace Purchase.Infrastructure
{
    public static class AddServiceBus
    {
        public static void AddEventBus(this IServiceCollection services)
        {
            //Receiver Queue
            services.AddSingleton<IEventBus, EventBusServiceBus.EventBusServiceBus>(sp =>
            {
                var serviceBusPersisterConnection = sp.GetRequiredService<IServiceBusPersisterConnection>();
                var iLifetimeScope = sp.GetRequiredService<ILifetimeScope>();
                var logger = sp.GetRequiredService<ILogger<EventBusServiceBus.EventBusServiceBus>>();
                var eventBusSubcriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();
                var purchaseQueueSettings = sp.GetRequiredService<IOptions<PurchaseQueueSettings>>().Value;
                var mediator = sp.GetRequiredService<IMediator>();   
                //var airtimePurchaseIntergrationEvent = sp.GetRequiredService<AirtimePurchaseIntegrationEvent>();
                //var iIntegrationEventHandler = sp.GetRequiredService<IIntegrationEventHandler<AirtimePurchaseIntegrationEvent>>();
                var serviceBusOptions = new ServiceBusOptions(
                    EventBusConnection: purchaseQueueSettings.EventBusConnection, 
                    SubscriptionClientName: "",// Null reference
                    TopicName: purchaseQueueSettings.QueueName,  // Removed the null
                    QueueName: purchaseQueueSettings.QueueName,
                    ReceiveMode: purchaseQueueSettings.ReceiveMode, 
                    PrefetchCount: purchaseQueueSettings.PrefetchCount,
                    AutoCompleteMessages: purchaseQueueSettings.AutoCompleteMessages,
                    MaxAutoLockRenewalDuration: purchaseQueueSettings.MaxAutoLockRenewalDuration,
                    MaxReceiveWaitTime: purchaseQueueSettings.MaxReceiveWaitTime,
                    MaxConcurrentCalls: purchaseQueueSettings.MaxConcurrentCalls, SubQueue: SubQueue.DeadLetter);

                return new EventBusServiceBus.EventBusServiceBus(serviceBusPersisterConnection, logger,
                    eventBusSubcriptionsManager, iLifetimeScope, serviceBusOptions, mediator);
            });
        }
    }
}
