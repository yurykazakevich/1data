using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Borigran.OneData.Platform.Http
{
    public class HttpHelper : IHttpHelper
    {
        private readonly ILogger<HttpHelper> logger;

        private readonly HttpClient client;

        public HttpHelper(ILogger<HttpHelper> logger)
        {
            this.logger = logger;

            var handler = new SocketsHttpHandler
            {
                PooledConnectionLifetime = TimeSpan.FromMinutes(2) // Recreate every 2 minutes
            };
            client = new HttpClient(handler);
        }

        public async Task<HttpResponseMessage> SendRequestAsync(HttpRequestMessage request)
        {
            try
            {
                var requestBody = "<hidden>";
#if DEBUG
                requestBody = await request.Content?.ReadAsStringAsync();
#endif

                logger.LogDebug("Send HTTPRequest: URL:{0}, Method: {1}, Headers:{2}, Body:{3}",
                    request.RequestUri, request.Method, request.Headers.Select(x => x.ToString()),
                    requestBody);

                var response = await client.SendAsync(request);
                var responseBody = "<hidden>";
#if DEBUG
                responseBody = await response.Content.ReadAsStringAsync();
#endif
                logger.LogDebug("Receive HttpResponse: Status:{0}, Content:{1}",
                    response.StatusCode, responseBody);

                return response;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "HTTP request failed: URL:{0}, Method: {1}",
                    request.RequestUri, request.Method);
                throw;
            }
        }
    }
}
