using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Borigran.OneData.WebApi.Pipeline
{
    public class ErrorHandlingMiddleWare
    {
        private readonly RequestDelegate next;
        private readonly IWebHostEnvironment env;
        private readonly ILogger<ErrorHandlingMiddleWare> logger;

        public ErrorHandlingMiddleWare(RequestDelegate next,
            IWebHostEnvironment env,
            ILogger<ErrorHandlingMiddleWare> logger)
        {
            this.next = next;
            this.env = env;
            this.logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next.Invoke(context);
            }
            catch(Exception ex)
            {
                LogException(ex);
                context.Response.Clear();
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "text/plain; charset=utf-8";

                if (env.IsDevelopment())
                {
                    await context.Response.WriteAsync(Environment.NewLine);
                    await context.Response.WriteAsync($"{ex.GetType().FullName}: {ex.Message}");
                    Exception inner = ex.InnerException;
                    while (inner != null)
                    {
                        await context.Response.WriteAsync(Environment.NewLine);
                        await context.Response.WriteAsync($"Inner Exception: {inner.GetType().FullName}: {inner.Message}");
                        inner = inner.InnerException;
                    }

                    await context.Response.WriteAsync(Environment.NewLine);
                    await context.Response.WriteAsync(ex.StackTrace);
                }
                else
                {
                    await context.Response.WriteAsync("Internal server error.");
                }
            }
        }

        private void LogException(Exception ex)
        {
            var errorId = new EventId(DateTime.Now.GetHashCode());

            logger.LogError(errorId, ex, ex.Message);
        }
    }
}
