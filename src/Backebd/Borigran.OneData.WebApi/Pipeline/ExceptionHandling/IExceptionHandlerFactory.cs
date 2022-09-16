using System;

namespace Borigran.OneData.WebApi.Pipeline.ExceptionHandling
{
    public interface IExceptionHandlerFactory
    {
        IExceptionHandler GetHandler<TException>(TException exception)
            where TException : Exception;
    }
}
