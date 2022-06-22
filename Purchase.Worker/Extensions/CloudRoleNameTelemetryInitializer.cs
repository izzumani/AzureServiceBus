using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;

namespace Purchase.Worker.Extensions
{
    public class CloudRoleNameTelemetryInitializer : ITelemetryInitializer
    {
        private readonly IConfiguration _configuration;
        public CloudRoleNameTelemetryInitializer(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public void Initialize(ITelemetry telemetry)
        {
            if (string.IsNullOrEmpty(telemetry.Context.Cloud.RoleName))
            {
                //set custom role name here
                telemetry.Context.Cloud.RoleName = _configuration["ApplicationInsights:Role:Name"];
                telemetry.Context.Cloud.RoleInstance = _configuration["ApplicationInsights:Role:Instance"];
            }
        }
    }
}
