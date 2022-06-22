using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Configuration;

namespace Purchase.NUnitTest.Extensions
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
                telemetry.Context.Cloud.RoleName = _configuration.GetValue<string>("ApplicationInsights:Role:Name");
                telemetry.Context.Cloud.RoleInstance = _configuration.GetValue<string>("ApplicationInsights:Role:Instance");
            }
        }
    }
}
