using Borigran.OneData.Platform.Helpers;
using FluentValidation;

namespace Borigran.OneData.WebApi.Models.Auth
{
    public class PhoneNumberRequestValidator : AbstractValidator<PhoneNumberRequest>
    {
        public PhoneNumberRequestValidator(IPhoneNumberHelper phoneNumberHelper)
        {
            RuleFor(x => x.PhoneNumber)
                .SetValidator(new PhoneNumberPropertyValidator<PhoneNumberRequest>(phoneNumberHelper));
        }
    }
}
