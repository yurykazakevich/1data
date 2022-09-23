using Borigran.OneData.Domain.Values;

namespace Borigran.OneData.Domain
{
    public abstract class ItemImage : EntityBase
    {
        public virtual BurialTypes BurialType { get; set; }
    }
}
