using FluentValidation;

namespace Borigran.OneData.WebApi.Models.Auth
{
    public class UserIdRequestValidator : AbstractValidator<UserIdRequest>
    {
        public UserIdRequestValidator()
        {
            RuleFor(x => x.UserId)
                .SetValidator(new UserIdPropertyValidator<UserIdRequest>());
        }
    }
}
