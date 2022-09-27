using Borigran.OneData.Domain.Entities;

namespace Borigran.OneData.Domain.NHibernate.Mapping
{
    public class ConstructorItemMap : MapBase<ConstructorItem>
    {
        public ConstructorItemMap()
            :base()
        {
            Map(x => x.Name).Not.Nullable();
            Map(x => x.ImageName).Nullable();
            Map(x => x.Price).Not.Nullable();
            Map(x => x.ArticleNumber).Not.Nullable();
            Map(x => x.Material).Not.Nullable();
            Map(x => x.Length).Not.Nullable();
            Map(x => x.Width).Not.Nullable();
            Map(x => x.Height).Nullable();
            Map(x => x.Weight).Not.Nullable();
            Map(x => x.Varranty).Not.Nullable();
            Map(x => x.ItemType).Not.Nullable()
                .CustomType<int>();

            References(x => x.Category)
                .Nullable()
                .Column("CategoryId")
                .ForeignKey("FK_ConstructorItem_Category")
                .Index("IDX_ConstructorItem_CategoryId")
                .Not.LazyLoad();

            Map(x => x.AllowedBurialTypes).Nullable();

            HasMany(x => x.PossiblePositions)
                .Not.LazyLoad();
        }
    }
}
