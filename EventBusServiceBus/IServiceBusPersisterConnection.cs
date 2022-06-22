namespace EventBusServiceBus;

public interface IServiceBusPersisterConnection : IDisposable
{
    ServiceBusClient ServiceBusClient { get; }
    ServiceBusAdministrationClient AdministrationClient { get; }
}