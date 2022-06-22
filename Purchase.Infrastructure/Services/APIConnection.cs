using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;

namespace Purchase.Infrastructure.Services
{
    public class APIConnection
    {
        private readonly ILogger<APIConnection> _logger;
        private readonly string _serviceAPIUrl;

        public APIConnection(ILogger<APIConnection> logger,string serviceAPIUrl)
        {
            _serviceAPIUrl = serviceAPIUrl;
            _logger = logger;
        }

        
        public async Task<HttpStatusCode> PostResponse(object requestBody)
        {
            Uri uri = null;

            HttpClient client = null;
            HttpResponseMessage result;

            string strStatusCode = string.Empty;
            string strContent = string.Empty;
            try
            {

                // string urlString = strDFCUServiceLink + MethodName;
                uri = new Uri(_serviceAPIUrl);

                HttpClientHandler clientHandler = new HttpClientHandler();
                clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

                using (client = new HttpClient(clientHandler))
                //using (client = new HttpClient())
                {
                    client.BaseAddress = uri;
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    client.Timeout = TimeSpan.FromMinutes(15);

                    ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback
                 (
                    delegate { return true; }
                 );

                    result = await client.PostAsJsonAsync<object>(uri, requestBody);
                    strContent = result.Content.ReadAsStringAsync().Result;
                }
                strStatusCode = result.StatusCode.ToString();
                return result.StatusCode;
            }
            catch (Exception ex)
            {
                strStatusCode = HttpStatusCode.InternalServerError.ToString();
                strContent= ex?.InnerException?.Message??ex?.Message;
                return HttpStatusCode.InternalServerError;
                



            }

            

           finally
            {
                
                _logger.LogInformation($"Request Parameter urlString: {JsonConvert.SerializeObject(uri)} " + Environment.NewLine +
                                            $"Client: {JsonConvert.SerializeObject(JsonConvert.SerializeObject(client))}" + Environment.NewLine +
                                            $"StatusCode: {JsonConvert.SerializeObject(strStatusCode)}" + Environment.NewLine +
                                            $"ResponseContent: {strContent}");
                _logger.LogInformation("==================================================================================");
            }





        }
    }
}
