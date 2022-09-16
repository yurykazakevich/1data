using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Borigran.OneData.WebApi.Pipeline.ExceptionHandling
{
    public abstract class ExceptioHandlerBase<TException, THandler> : IExceptionHandler<TException>
        where TException : Exception
    {
        protected readonly ILogger<THandler> logger;
        protected readonly IWebHostEnvironment environment;

        protected ExceptioHandlerBase(IWebHostEnvironment environment, ILogger<THandler> logger)
        {
            this.environment = environment;
            this.logger = logger;
        }

        public abstract Task HandleTypedExceptionAsync(HttpResponse response, TException exception);

        public async Task HandleExceptionAsync(HttpResponse response, Exception exception)
        {
            var typedException = exception as TException;
            if (typedException != null)
            {
                await HandleTypedExceptionAsync(response, typedException);
            }
            else
            {
                logger.LogError($"Couldnot cast {exception.GetType()} to {typeof(TException)}");
            }
        }

        protected virtual void LogException(Exception ex)
        {
            var errorId = GenerateErrorId();

            logger.LogError(errorId, ex, ex.Message);
        }

        protected EventId GenerateErrorId()
        {
            return new EventId(DateTime.Now.GetHashCode());
        }
    }
}
