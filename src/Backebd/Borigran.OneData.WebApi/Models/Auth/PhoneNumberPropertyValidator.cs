using Borigran.OneData.Platform.Helpers;
using FluentValidation;
using FluentValidation.Validators;

namespace Borigran.OneData.WebApi.Models.Auth
{
    public class PhoneNumberPropertyValidator<TRequest> : PropertyValidator<TRequest, string>
        where TRequest : PhoneNumberRequest
    {
        public readonly IPhoneNumberHelper phoneNumberHelper;
        
        public override string Name => "PhoneNumberPropertyValidator";

        public PhoneNumberPropertyValidator(IPhoneNumberHelper phoneNumberHelper)
        {
            this.phoneNumberHelper = phoneNumberHelper;
        }

        public override bool IsValid(ValidationContext<TRequest> context, string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                context.MessageFormatter.AppendArgument("Issue", "должно быть заполнено");
                return false;
            }
            else if(!phoneNumberHelper.ValidatePhoneNumber(value))
            {
                context.MessageFormatter.AppendArgument("Issue", "имеет неверный формат");
                return false;
            }

            return true;
        }

        protected override string GetDefaultMessageTemplate(string errorCode)
        {
            return "Поле 'Номер телефона' {Issue}";   
        }
    }
}
