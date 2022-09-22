namespace Borigran.OneData.Domain.Entities
{
    public class ConstructorItem : EntityBase
    {
        public virtual IList<ConstructorItemImage> Images { get; set; } 

        public virtual string Name { get; set; }

        public virtual decimal Price { get; set; }

        public virtual string ArticleNumber { get; set; }

        public virtual string Material { get; set; }

        public virtual decimal Length { get; set; }

        public virtual decimal Width { get; set; }

        public virtual decimal Height { get; set; }

        public virtual decimal Weight { get; set; }

        public virtual int Varranty { get; set; }

        public ConstructorItem()
        {
            Images = new List<ConstructorItemImage>();
        }
    }
}
