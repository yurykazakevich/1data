using System;

namespace Borigran.OneData.WebApi.Pipeline.ExceptionHandling
{
    public class ExceptionHandlerFactory : IExceptionHandlerFactory
    {
        private readonly IServiceProvider serviceProvider;

        public ExceptionHandlerFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public IExceptionHandler GetHandler<TException>(TException exception) where TException : Exception
        {
            var handlerType = typeof(IExceptionHandler<>).MakeGenericType(exception.GetType());
            var handler = serviceProvider.GetService(handlerType) as IExceptionHandler;
            if(handler == null)
            {
                handler = serviceProvider.GetService(typeof(IExceptionHandler<Exception>)) 
                    as IExceptionHandler;
            }

            return handler;
        }
    }
}
