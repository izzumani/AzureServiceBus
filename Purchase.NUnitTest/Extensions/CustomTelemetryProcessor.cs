using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights.Extensibility.Implementation;
using Microsoft.AspNetCore.Http;

namespace Purchase.NUnitTest.Extensions
{
    public class CustomTelemetryProcessor : ITelemetryProcessor
    {
        private ITelemetryProcessor _next;
        private IHttpContextAccessor _httpContextAccessor;

        public CustomTelemetryProcessor(ITelemetryProcessor next, IHttpContextAccessor httpContextAccessor)
        {
            // never gets to here
            _next = next;
            _httpContextAccessor = httpContextAccessor;
        }

        public void Process(ITelemetry item)
        {

            if (item is RequestTelemetry request)
            {
                //for testing purpose, I just add custom property to trace telemetry, you can modify the code as per your need.
                //if (item is TraceTelemetry traceTelemetry)
                //{
                // use _httpContextAccessor here...        
                if (_httpContextAccessor.HttpContext != null)
                {
                    request.Properties.Add("RequestMessageBody",
                        _httpContextAccessor.HttpContext.Request.Body.ToString());
                    request.Properties.Add("ResponseMessageBody",
                        _httpContextAccessor.HttpContext.Response.Body.ToString());
                }
            }

            // Send the item to the next TelemetryProcessor
            _next.Process(item);

        }
    }
}
