using FluentValidation;
using FluentValidation.Validators;
using System.Text.RegularExpressions;

namespace Borigran.OneData.WebApi.Models.Auth
{
    public class PhoneNumberPropertyValidator<TRequest> : PropertyValidator<TRequest, string>
        where TRequest : PhoneNumberRequest
    {
        public readonly Regex PhoneNumberRegex = new Regex(@"^\+\d{1-3}\({0-1}\d{3}\){0-1}\d{7}");
        public override string Name => "PhoneNumberPropertyValidator";

        public override bool IsValid(ValidationContext<TRequest> context, string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                context.MessageFormatter.AppendArgument("Issue", "должно быть заполнено");
                return false;
            }
            else if(!PhoneNumberRegex.IsMatch(value))
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
