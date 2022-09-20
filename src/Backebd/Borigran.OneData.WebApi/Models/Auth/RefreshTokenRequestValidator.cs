using FluentValidation;

namespace Borigran.OneData.WebApi.Models.Auth
{
    public class RefreshTokenRequestValidator : AbstractValidator<RefreshTokenRequest>
    {
        public RefreshTokenRequestValidator()
        {
            RuleFor(x => x.PhoneNumber).SetValidator(new PhoneNumberPropertyValidator<RefreshTokenRequest>());
            RuleFor(x => x.RefreshToken).NotEmpty();
            RuleFor(x => x.ExpiredToken).NotEmpty();
        }
    }
}
