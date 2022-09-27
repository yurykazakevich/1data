using Borigran.OneData.Domain.Values;

namespace Borigran.OneData.Domain.Entities
{
    public class ConstructorItemCategory : EntityBase
    {
        public virtual string Name { get; set; }

        public virtual CItemTypes ItemType { get; set; }

        public virtual int SortOrder { get; set; }

        public virtual ConstructorItemCategory ParentCategory { get; set; }
    }
}
