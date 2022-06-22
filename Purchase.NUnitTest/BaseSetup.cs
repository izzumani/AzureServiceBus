using EventBusServiceBus;
using MediatR;
using Microsoft.ApplicationInsights.DependencyCollector;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.QuickPulse;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Purchase.Application;
using Purchase.Infrastructure.Settings;
using Purchase.Core.Events;
using Purchase.Infrastructure;
using Purchase.NUnitTest.Extensions;
using System.Reflection;

public abstract class BaseTest
{
    private static IConfigurationRoot _configuration;
    private static IServiceScopeFactory _scopeFactory;
    [SetUp]
    public void SetUp()
    {
        //Do generic Stuff 

        var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .AddEnvironmentVariables();

        _configuration = builder.Build();
        var services = new ServiceCollection();
        
        services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());
        //services.AddMediatR(Assembly.GetExecutingAssembly());


        services.Configure<PurchaseQueueSettings>(_configuration.GetSection("PurchaseQueueSettings"));
        services.AddSingleton<IServiceBusPersisterConnection>(sp =>
        {
            var serviceBusConnectionString = _configuration.GetValue<string>("PurchaseQueueSettings:EventBusConnection");

            return new DefaultServiceBusPersisterConnection(serviceBusConnectionString);
        });

        services.AddSingleton<ITelemetryInitializer>(sp =>
        {
          
            return new CloudRoleNameTelemetryInitializer(_configuration);
        });

        services.AddEventBus();

        //services.AddSingleton< ITelemetryInitializer, CloudRoleNameTelemetryInitializer>();
        services.AddApplicationInsightsTelemetryWorkerService(_configuration.GetConnectionString("ApplicationInsights"));
        services.ConfigureTelemetryModule<DependencyTrackingTelemetryModule>((module, o) =>
        {
            module.EnableSqlCommandTextInstrumentation = true;
        });
        services.AddHealthCheck(_configuration);
        
        services.AddApplication(_configuration);
        services.AddInfrastructure(_configuration); 

        _scopeFactory = services.BuildServiceProvider().GetService<IServiceScopeFactory>();
    }

    [TearDown]
    public void TearDown()
    {
        // Do generic stuff 
    }

    protected static async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
    {
        using var scope = _scopeFactory.CreateScope();

        var mediator = scope.ServiceProvider.GetService<ISender>();

        return await mediator.Send(request);
    }

   

    protected INotification GetNotificationCorrespondingToDomainEvent(DomainEvent domainEvent)
    {
        return (INotification)Activator.CreateInstance(
            typeof(DomainEventNotification<>).MakeGenericType(domainEvent.GetType()), domainEvent);
    }


}




