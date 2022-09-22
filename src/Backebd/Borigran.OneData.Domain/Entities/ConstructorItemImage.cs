namespace Borigran.OneData.Domain.Entities
{
    public class ConstructorItemImage : EntityBase
    {
        public virtual string ImagePath { get; set; }

        public virtual ConstructorItem Item { get; set; }
    }
}
