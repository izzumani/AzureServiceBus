namespace EventBusServiceBus;

public class DefaultServiceBusPersisterConnection : IServiceBusPersisterConnection
{
    private readonly string _serviceBusConnectionString;
    private ServiceBusClient _serviceBusClient;
    private readonly ServiceBusAdministrationClient _subscriptionClient;

    bool _disposed;

    public DefaultServiceBusPersisterConnection(string serviceBusConnectionString)
    {
        _serviceBusConnectionString = serviceBusConnectionString;
        _subscriptionClient = new ServiceBusAdministrationClient(_serviceBusConnectionString);
        _serviceBusClient = new ServiceBusClient(_serviceBusConnectionString);
    }

    public ServiceBusClient ServiceBusClient
    {
        get
        {
            if (_serviceBusClient.IsClosed)
            {
                _serviceBusClient = new ServiceBusClient(_serviceBusConnectionString);
            }
            return _serviceBusClient;
        }
    }

    public ServiceBusAdministrationClient AdministrationClient
    {
        get
        {
            return _subscriptionClient;
        }
    }

    public ServiceBusClient CreateModel()
    {
        if (_serviceBusClient.IsClosed)
        {
            _serviceBusClient = new ServiceBusClient(_serviceBusConnectionString);
        }

        return _serviceBusClient;
    }

    public void Dispose()
    {
        if (_disposed) return;

        _disposed = true;
        _serviceBusClient.DisposeAsync().GetAwaiter().GetResult();
    }
}
