using Borigran.OneData.Platform.Helpers;
using FluentValidation;

namespace Borigran.OneData.WebApi.Models.Auth
{
    public class RefreshTokenRequestValidator : AbstractValidator<RefreshTokenRequest>
    {
        public RefreshTokenRequestValidator(IPhoneNumberHelper phoneNumberHelper)
        {
            RuleFor(x => x.PhoneNumber)
                .SetValidator(new PhoneNumberPropertyValidator<RefreshTokenRequest>(phoneNumberHelper));
            RuleFor(x => x.ExpiredToken).NotEmpty();
        }
    }
}
