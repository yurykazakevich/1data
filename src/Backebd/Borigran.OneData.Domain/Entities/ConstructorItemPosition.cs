using Borigran.OneData.Domain.Values;

namespace Borigran.OneData.Domain.Entities
{
    public class ConstructorItemPosition : EntityBase
    {
        public virtual string ImageName { get; set; }

        public virtual ItemPositions Position { get; set; }

        public virtual ConstructorItem Item { get; set; }
    }
}
