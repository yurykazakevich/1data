using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Borigran.OneData.WebApi.Pipeline.ExceptionHandling.Handlers
{
    public class DefaultExceptionHandler : ExceptioHandlerBase<Exception, DefaultExceptionHandler>
    {
        public DefaultExceptionHandler(IWebHostEnvironment environment, ILogger<DefaultExceptionHandler> logger)
            : base(environment, logger)
        {
        }

        public override async Task HandleTypedExceptionAsync(HttpResponse response, Exception exception)
        {
            response.Clear();
            response.StatusCode = StatusCodes.Status500InternalServerError;
            response.ContentType = "text/plain; charset=utf-8";

            if (environment.IsDevelopment())
            {
                await response.WriteAsync(Environment.NewLine);
                await response.WriteAsync($"{exception.GetType().FullName}: {exception.Message}");
                Exception inner = exception.InnerException;
                while (inner != null)
                {
                    await response.WriteAsync(Environment.NewLine);
                    await response.WriteAsync($"Inner Exception: {inner.GetType().FullName}: {inner.Message}");
                    inner = inner.InnerException;
                }

                await response.WriteAsync(Environment.NewLine);
                await response.WriteAsync(exception.StackTrace);
            }
            else
            {
                await response.WriteAsync("Internal server error.");
            }
        }
    }
}
