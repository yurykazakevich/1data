using FluentValidation;

namespace Borigran.OneData.WebApi.Models.Auth
{
    public class PhoneNumberRequestValidator : AbstractValidator<PhoneNumberRequest>
    {
        public PhoneNumberRequestValidator()
        {
            RuleFor(x => x.PhoneNumber).SetValidator(new PhoneNumberPropertyValidator<PhoneNumberRequest>());
        }
    }
}
