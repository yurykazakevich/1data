using Borigran.OneData.Domain.Values;

namespace Borigran.OneData.Domain.Entities
{
    public class ConstructorItem : EntityBase
    {
        public virtual string Name { get; set; }

        public virtual decimal Price { get; set; }

        public virtual string ArticleNumber { get; set; }

        public virtual string Material { get; set; }

        public virtual int Length { get; set; }

        public virtual int Width { get; set; }

        public virtual int Height { get; set; }

        public virtual decimal Weight { get; set; }

        public virtual int Varranty { get; set; }

        public virtual CItemTypes ItemType { get; set; }

        public virtual ConstructorItemCategory? Category { get; set; }

        public virtual string AllowedBurialTypes { get; set; }

        public virtual IEnumerable<ConstructorItemPosition> PossiblePositions { get; set; }

        public ConstructorItem()
        {
            PossiblePositions = new List<ConstructorItemPosition>();
        }

    }
}
