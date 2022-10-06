using FluentValidation;

namespace Borigran.OneData.WebApi.Models.Auth
{
    public class RefreshTokenRequestValidator : AbstractValidator<RefreshTokenRequest>
    {
        public RefreshTokenRequestValidator()
        {
            RuleFor(x => x.UserId)
                .SetValidator(new UserIdPropertyValidator<RefreshTokenRequest>());
            RuleFor(x => x.ExpiredToken).NotEmpty();
        }
    }
}
