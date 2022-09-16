using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Borigran.OneData.WebApi.Pipeline.ExceptionHandling
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate next;
        private readonly IExceptionHandlerFactory exceptionHandlerFactory;
        private readonly ILogger<ExceptionHandlingMiddleware> logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, 
            IExceptionHandlerFactory exceptionHandlerFactory,
            ILogger<ExceptionHandlingMiddleware> logger)
        {
            this.next = next;
            this.exceptionHandlerFactory = exceptionHandlerFactory;
            this.logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next.Invoke(context);
            }
            catch (Exception ex)
            {
                try
                {
                    var handler = exceptionHandlerFactory.GetHandler(ex);
                    await handler.HandleExceptionAsync(context.Response, ex);
                }
                catch (Exception handlerException)
                {
                    logger.LogError(handlerException, "Could not handle exception.");
                    logger.LogError(ex, "The original exception is:");

                    context.Response.Clear();
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                }
            }
        }
    }
}
