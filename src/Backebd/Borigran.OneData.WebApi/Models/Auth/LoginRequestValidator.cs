using FluentValidation;

namespace Borigran.OneData.WebApi.Models.Auth
{
    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(x => x.PhoneNumber).SetValidator(new PhoneNumberPropertyValidator<LoginRequest>());
            RuleFor(x => x.VerificationCode)
                .NotEmpty()
                .Matches("[1-9]{1}[0-9]{5}")
                .WithMessage("Поле 'Код' должно содержать строку из 6-ти символов");
            RuleFor(x => x.UserProvidedCode)
                .NotEmpty();
        }
    }
}
