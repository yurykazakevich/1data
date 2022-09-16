using Borigran.OneData.WebApi.Models.ErrorResponses;
using FluentValidation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace Borigran.OneData.WebApi.Pipeline.ExceptionHandling.Handlers
{
    public class ValidationExceptionHandler : ExceptioHandlerBase<ValidationException, ValidationExceptionHandler>
    {
        public ValidationExceptionHandler(IWebHostEnvironment environment, ILogger<ValidationExceptionHandler> logger) 
            : base(environment, logger)
        {
        }

        public override async Task HandleTypedExceptionAsync(HttpResponse response, ValidationException exception)
        {
            response.Clear();
            response.StatusCode = StatusCodes.Status400BadRequest;
            response.ContentType = "application/json";

            var errorResponse = new ValidationErrorResponse
            {
                Errors = exception.Errors.Select(x => new ValidationErrorResponseItem
                {
                    Message = x.ErrorMessage,
                    PropertyName = x.PropertyName
                }).ToList()
            };

            await response.WriteAsJsonAsync(errorResponse);
        }
    }
}
