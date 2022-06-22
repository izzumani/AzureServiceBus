using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using Purchase.Infrastructure.Settings;

namespace Purchase.NUnitTest.Extensions
{
    public static class HealthCheckInitializer
    {
        public static IServiceCollection AddHealthCheck(this IServiceCollection services,
            IConfiguration configuration)
        {
            var hcBuilder = services.AddHealthChecks();

            hcBuilder.AddCheck("self", () => HealthCheckResult.Healthy());

            var serviceProvider = services.BuildServiceProvider();

            var purchaseQueueSettings = serviceProvider.GetRequiredService<IOptions<PurchaseQueueSettings>>().Value;

            hcBuilder.AddSqlServer(
                    configuration["ConnectionString"],
                    name: "purchaseDB-check",
                    tags: new string[] { "purchasedb" });


            if (purchaseQueueSettings.QueueName != null)
                hcBuilder.AddAzureServiceBusQueue(
                    purchaseQueueSettings.EventBusConnection,
                    queueName: purchaseQueueSettings.QueueName,
                    name: "purchase-queue-check",
                    tags: new string[] { "servicebus" });

            return services;
        }
    }
}
