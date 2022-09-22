using Borigran.OneData.Platform.Helpers;
using FluentValidation;

namespace Borigran.OneData.WebApi.Models.Auth
{
    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator(IPhoneNumberHelper phoneNumberHelper)
        {
            RuleFor(x => x.PhoneNumber).SetValidator(new PhoneNumberPropertyValidator<LoginRequest>(phoneNumberHelper));
            RuleFor(x => x.UserProvidedCode)
                .NotEmpty()
                .Matches("[1-9]{1}[0-9]{5}")
                .WithMessage("Поле 'Код' должно содержать строку из 6-ти символов");
            RuleFor(x => x.VerificationCode)
                .NotEmpty();
        }
    }
}
