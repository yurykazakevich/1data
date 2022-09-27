using Borigran.OneData.Domain.Entities;

namespace Borigran.OneData.Domain.NHibernate.Mapping
{
    public class ConstructorItemCategoryMap : MapBase<ConstructorItemCategory>
    {
        public ConstructorItemCategoryMap()
            :base()
        {
            Map(x => x.Name).Not.Nullable();
            Map(x => x.ItemType).Not.Nullable()
                .CustomType<int>();
            Map(x => x.SortOrder).Nullable();

            References(x => x.ParentCategory)
                .Nullable()
                .Column("ParentCategoryId")
                .ForeignKey("FK_ConstructorItemCategory_ParentCategory")
                .Index("IDX_ConstructorItemCategory_ParentCategoryId")
                .Not.LazyLoad();
        }
    }
}
