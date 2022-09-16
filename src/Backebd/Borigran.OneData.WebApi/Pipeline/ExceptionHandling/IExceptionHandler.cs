using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace Borigran.OneData.WebApi.Pipeline.ExceptionHandling
{
    public interface IExceptionHandler
    {
        Task HandleExceptionAsync(HttpResponse response, Exception exception);
    }
    public interface IExceptionHandler<TException> : IExceptionHandler
        where TException: Exception
    {
        Task HandleTypedExceptionAsync(HttpResponse response, TException exception);
    }
}
