using FluentValidation;

namespace Borigran.OneData.WebApi.Models.Auth
{
    public class PhoneNumberRequestValidator : AbstractValidator<PhoneNumberRequest>
    {
        public PhoneNumberRequestValidator()
        {
            RuleFor(x => x.PhoneNumber)
                .NotEmpty()
                .Matches(@"^\+(\d){11-12}");
        }
    }
}
