using EventBusServiceBus;
using Microsoft.ApplicationInsights.DependencyCollector;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.QuickPulse;
using Microsoft.AspNetCore.Http;
using Purchase.Application;
using Purchase.Infrastructure.Settings;
using Purchase.Infrastructure;
using Purchase.Worker;
using Purchase.Worker.Extensions;
using MediatR;
using System.Reflection;
using Autofac.Extensions.DependencyInjection;

//using Purchase.Worker.MiddleWare; This does not exists anywere in the project


IHost host = Host.CreateDefaultBuilder(args)
   
    .ConfigureServices((hostContext, services) =>
    {
        IConfiguration configuration = hostContext.Configuration;
        //services.AddMediatR(Assembly.GetExecutingAssembly());
        //services.AddMediatR(typeof(Worker).Assembly);
        services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());
        services.Configure<PurchaseQueueSettings>(configuration.GetSection("PurchaseQueueSettings"));
        services.AddSingleton<IServiceBusPersisterConnection>(sp =>
        {
            var serviceBusConnectionString = hostContext.Configuration["PurchaseQueueSettings:EventBusConnection"];

            return new DefaultServiceBusPersisterConnection(serviceBusConnectionString);
        });
        services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();
        services.AddEventBus();

        services.AddSingleton<ITelemetryInitializer, CloudRoleNameTelemetryInitializer>();
        
        services.AddApplicationInsightsTelemetryWorkerService(hostContext.Configuration.GetConnectionString("ApplicationInsights"));
        services.ConfigureTelemetryModule<DependencyTrackingTelemetryModule>((module, o) =>
        {
            module.EnableSqlCommandTextInstrumentation = true;
        });
        
        services.AddHealthCheck(hostContext.Configuration);
        services.AddHostedService<Worker>();
        services.AddApplication(hostContext.Configuration);
        services.AddInfrastructure(hostContext.Configuration);
        





    })
    .UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .Build();

await host.RunAsync();
