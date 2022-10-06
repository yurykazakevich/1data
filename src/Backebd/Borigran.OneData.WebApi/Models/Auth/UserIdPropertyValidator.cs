using FluentValidation;
using FluentValidation.Validators;

namespace Borigran.OneData.WebApi.Models.Auth
{
    public class UserIdPropertyValidator<TRequest> : PropertyValidator<TRequest, int>
        where TRequest : UserIdRequest
    {       
        public override string Name => "UserIdPropertyValidator";

        public override bool IsValid(ValidationContext<TRequest> context, int value)
        {
            if (value <= 0)
            {
                context.MessageFormatter.BuildMessage("UserId должен быть больше нуля");
                return false;
            }

            return true;
        }
    }
}
