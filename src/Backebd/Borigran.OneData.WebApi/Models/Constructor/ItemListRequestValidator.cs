using FluentValidation;

namespace Borigran.OneData.WebApi.Models.Constructor
{
    public class ItemListRequestValidator : AbstractValidator<ItemListRequest>
    {
        public ItemListRequestValidator()
        {
            RuleFor(x => x.ItemType).NotEmpty();
            RuleFor(x => x.BurialType).NotEmpty();
            RuleFor(x => x.BurialPosition).NotEmpty();
        }
    }
}
