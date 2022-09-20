using Borigran.OneData.WebApi.Models.ErrorResponses;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Text;
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

            var errorResponse = new DefaultErrorResponse
            {
                Code = response.StatusCode
            };

            if (environment.IsDevelopment())
            {
                var errorMessageBuilder = new StringBuilder();
                errorMessageBuilder.AppendLine($"{exception.GetType().FullName}: {exception.Message}");
                Exception inner = exception.InnerException;
                while (inner != null)
                {
                    errorMessageBuilder.AppendLine($"Inner Exception: {inner.GetType().FullName}: {inner.Message}");
                    inner = inner.InnerException;
                }

                errorMessageBuilder.AppendLine(exception.StackTrace);
                errorResponse.Error = errorMessageBuilder.ToString();
            }
            else
            {
                errorResponse.Error = "Internal server error.";
            }

            await response.WriteAsJsonAsync(errorResponse);
        }
    }
}
