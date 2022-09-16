using Autofac;
using Borigran.OneData.WebApi.Models.Auth;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Borigran.OneData.WebApi.Pipeline
{
    public class FluentValidationFilter : Attribute, IAsyncActionFilter
    {
        private readonly IServiceProvider serviceProvider;

        public FluentValidationFilter(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var errorList = new List<ValidationFailure>();

            foreach(var request in context.ActionArguments.Values)
            {
                var validator = FindRequestValidator(request);
                if(validator != null)
                {
                    var validationContext = CreateValidationContext(request);
                    var validationResult = await ((IValidator)validator).ValidateAsync(validationContext);
                    if(!validationResult.IsValid)
                    {
                        errorList.AddRange(validationResult.Errors);
                    }
                }
            }

            if(errorList.Any())
            {
                throw new ValidationException(errorList);
            }

            await next();
        }

        private object FindRequestValidator(object request)
        {
            var validatorType = typeof(IValidator<>).MakeGenericType(request.GetType());

            var validator = serviceProvider.GetService(validatorType);

            return validator;
        }

        private IValidationContext CreateValidationContext(object request)
        {
            var validationContextType = typeof(ValidationContext<>).MakeGenericType(request.GetType());
            var validationContext = Activator.CreateInstance(validationContextType, request)
                as IValidationContext;

            return validationContext;
        }
    }
}
