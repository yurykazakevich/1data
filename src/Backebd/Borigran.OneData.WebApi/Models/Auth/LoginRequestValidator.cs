using FluentValidation;

namespace Borigran.OneData.WebApi.Models.Auth
{
    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(x => x.VerificationCode)
                .NotEmpty()
                .Matches("[1-9]{1}[0-9]{5}");

            RuleFor(x => x.UserProvidedCode)
                .NotEmpty();
        }
    }
}
