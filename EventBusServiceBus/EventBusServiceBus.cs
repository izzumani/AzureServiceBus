using EventBusServiceBus.Abstractions;
using EventBusServiceBus.EventDTOs;
using MediatR;
using Newtonsoft.Json;

namespace EventBusServiceBus;

public class EventBusServiceBus : IEventBus, IDisposable
{
    private readonly IServiceBusPersisterConnection _serviceBusPersisterConnection;
    private readonly ILogger<EventBusServiceBus> _logger;
    private readonly IEventBusSubscriptionsManager _subsManager;
    private readonly ILifetimeScope _autofac;
    private readonly string _topicName = "_event_bus";
    private readonly string _queueName = "_event_bus";
    private readonly string _subscriptionName;
    private ServiceBusSender _sender;
    private ServiceBusProcessor _processor;
    private readonly string AUTOFAC_SCOPE_NAME = "event_bus";
    private const string INTEGRATION_EVENT_SUFFIX = "IntegrationEvent";
    private readonly IMediator _mediator;
    

    public EventBusServiceBus(IServiceBusPersisterConnection serviceBusPersisterConnection,
        ILogger<EventBusServiceBus> logger, IEventBusSubscriptionsManager subsManager, 
        ILifetimeScope autofac, ServiceBusOptions serviceBusOptions,
         IMediator mediator

        )
    {
        _topicName = serviceBusOptions.TopicName;
        _subscriptionName = serviceBusOptions.SubscriptionClientName;
        _serviceBusPersisterConnection = serviceBusPersisterConnection;
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _subsManager = subsManager ?? new InMemoryEventBusSubscriptionsManager();
        _autofac = autofac;
        _mediator = mediator;
        ServiceBusProcessorOptions options = new ServiceBusProcessorOptions
        {
            MaxConcurrentCalls = serviceBusOptions.MaxConcurrentCalls,
            AutoCompleteMessages = serviceBusOptions.AutoCompleteMessages,
            PrefetchCount = serviceBusOptions.PrefetchCount, 
            SubQueue = serviceBusOptions.SubQueue,
            ReceiveMode = (ServiceBusReceiveMode)serviceBusOptions.ReceiveMode
        };
        if (serviceBusOptions.TopicName != null)
        {
            _sender = _serviceBusPersisterConnection.ServiceBusClient.CreateSender(_topicName);
            _processor =
                _serviceBusPersisterConnection.ServiceBusClient.CreateProcessor(_topicName, _subscriptionName, options);
        }
        else if (serviceBusOptions.QueueName != null)
        {
            _queueName = serviceBusOptions.QueueName;
            _sender = _serviceBusPersisterConnection.ServiceBusClient.CreateSender(_queueName);
            _processor = _serviceBusPersisterConnection.ServiceBusClient.CreateProcessor(_queueName, options);
            
        }

        //RemoveDefaultRule();
        //RegisterSubscriptionClientMessageHandlerAsync().GetAwaiter().GetResult();
    }

    public void Publish(IntegrationEvent @event)
    {
        var eventName = @event.GetType().Name.Replace(INTEGRATION_EVENT_SUFFIX, "");
        var jsonMessage = System.Text.Json.JsonSerializer.Serialize(@event, @event.GetType());
        
        var body = Encoding.UTF8.GetBytes(jsonMessage);

        var message = new ServiceBusMessage
        {
            MessageId = Guid.NewGuid().ToString(),
            Body = new BinaryData(body),
            Subject = eventName,
        };

        _sender.SendMessageAsync(message)
            .GetAwaiter()
            .GetResult();
    }

    public void SubscribeDynamic<TH>(string eventName)
        where TH : IDynamicIntegrationEventHandler
    {
        _logger.LogInformation("Subscribing to dynamic event {EventName} with {EventHandler}", eventName, typeof(TH).Name);

        _subsManager.AddDynamicSubscription<TH>(eventName);

        RegisterSubscriptionClientMessageHandlerAsync().GetAwaiter().GetResult();
    }

    public void Subscribe<T, TH>()
        where T : IntegrationEvent
        where TH : IIntegrationEventHandler<T>
    {
        var eventName = typeof(T).Name.Replace(INTEGRATION_EVENT_SUFFIX, "");

        var containsKey = _subsManager.HasSubscriptionsForEvent<T>();
        if (!containsKey)
        {
            try
            {
                //RemoveDefaultRule();
                _serviceBusPersisterConnection.AdministrationClient.CreateRuleAsync(_topicName, _subscriptionName, new CreateRuleOptions
                {
                    Filter = new CorrelationRuleFilter() { Subject = eventName },
                    Name = eventName
                }).GetAwaiter().GetResult();

                
                RegisterSubscriptionClientMessageHandlerAsync().GetAwaiter().GetResult();
            }
            catch (ServiceBusException)
            {
                _logger.LogWarning("The messaging entity {eventName} already exists.", eventName);
            }
        }

        _logger.LogInformation("Subscribing to event {EventName} with {EventHandler}", eventName, typeof(TH).Name);

        _subsManager.AddSubscription<T, TH>();
    }

    public void Unsubscribe<T, TH>()
        where T : IntegrationEvent
        where TH : IIntegrationEventHandler<T>
    {
        var eventName = typeof(T).Name.Replace(INTEGRATION_EVENT_SUFFIX, "");

        try
        {
            _serviceBusPersisterConnection
                .AdministrationClient
                .DeleteRuleAsync(_topicName, _subscriptionName, eventName)
                .GetAwaiter()
                .GetResult();
        }
        catch (ServiceBusException ex) when (ex.Reason == ServiceBusFailureReason.MessagingEntityNotFound)
        {
            _logger.LogWarning("The messaging entity {eventName} Could not be found.", eventName);
        }

        _logger.LogInformation("Unsubscribing from event {EventName}", eventName);

        _subsManager.RemoveSubscription<T, TH>();
    }

    public void UnsubscribeDynamic<TH>(string eventName)
        where TH : IDynamicIntegrationEventHandler
    {
        _logger.LogInformation("Unsubscribing from dynamic event {EventName}", eventName);

        _subsManager.RemoveDynamicSubscription<TH>(eventName);
    }

    private async Task RegisterSubscriptionClientMessageHandlerAsync()
    {
        
        _processor.ProcessMessageAsync +=
            async (args) =>
            {
                

                var eventName = $"{args.Message.Subject}{INTEGRATION_EVENT_SUFFIX}";
                _logger.LogDebug($"Executing Eventname: {eventName}");
                string messageData = args.Message.Body.ToString();

                _logger.LogDebug($"Executing MessageData: {messageData}");
                // Complete the message so that it is not received again.
                if (await ProcessEvent(eventName, messageData))
                {
                    AirtimePurchaseDTO airtimePurchaseDTO = JsonConvert.DeserializeObject<AirtimePurchaseDTO>(messageData);

                    AirtimePurchaseIntegrationEvent<AirtimePurchaseDTO> airtimePurchaseEvent = new AirtimePurchaseIntegrationEvent<AirtimePurchaseDTO>(airtimePurchaseDTO);


                    await _mediator.Publish((INotification)Activator.CreateInstance(
                   typeof(IntegrationEventEventNotification<>).MakeGenericType(airtimePurchaseEvent.GetType()), airtimePurchaseEvent));


                    //await _mediator.Publish(new AirtimePurchaseQueueingCommand(airtimePurchaseDTO));
                    _logger.LogDebug($"Completed Message for EventName: {eventName} with MessageData: {messageData}");
                    await args.CompleteMessageAsync(args.Message);
                }
            };

        _processor.ProcessErrorAsync += ErrorHandler;
        await _processor.StartProcessingAsync();
    }

    public void Dispose()
    {
        _subsManager.Clear();
        _processor.CloseAsync().GetAwaiter().GetResult();
    }

    private Task ErrorHandler(ProcessErrorEventArgs args)
    {
        var ex = args.Exception;
        var context = args.ErrorSource;

        _logger.LogError(ex, "ERROR handling message: {ExceptionMessage} - Context: {@ExceptionContext}", ex.Message, context);

        return Task.CompletedTask;
    }

    private async Task<bool> ProcessEvent(string eventName, string message)
    {

        _logger.LogDebug($"Processing Event: {eventName} with Message Body: {message}");
        var processed = false;
        if (_subsManager.HasSubscriptionsForEvent(eventName))
        {
            using (var scope = _autofac.BeginLifetimeScope(AUTOFAC_SCOPE_NAME))
            {
                var subscriptions = _subsManager.GetHandlersForEvent(eventName);
                foreach (var subscription in subscriptions)
                {
                    if (subscription.IsDynamic)
                    {
                        var handler = scope.ResolveOptional(subscription.HandlerType) as IDynamicIntegrationEventHandler;
                        if (handler == null) continue;
                        
                        using dynamic eventData = JsonDocument.Parse(message);
                        await handler.Handle(eventData);
                    }
                    else
                    {
                        var handler = scope.ResolveOptional(subscription.HandlerType);
                        if (handler == null) continue;
                        var eventType = _subsManager.GetEventTypeByName(eventName);
                        var integrationEvent = System.Text.Json.JsonSerializer.Deserialize(message, eventType);
                        var concreteType = typeof(IIntegrationEventHandler<>).MakeGenericType(eventType);
                        await (Task)concreteType.GetMethod("Handle").Invoke(handler, new object[] { integrationEvent });
                    }
                }
            }
            processed = true;
        }
        return processed;
    }

    private void RemoveDefaultRule()
    {
        try
        {
            _serviceBusPersisterConnection
                .AdministrationClient
                .DeleteRuleAsync(_topicName, _subscriptionName, RuleProperties.DefaultRuleName)
                .GetAwaiter()
                .GetResult();
        }
        catch (ServiceBusException ex) when (ex.Reason == ServiceBusFailureReason.MessagingEntityNotFound)
        {
            _logger.LogWarning("The messaging entity {DefaultRuleName} Could not be found.", RuleProperties.DefaultRuleName);
        }
    }


}