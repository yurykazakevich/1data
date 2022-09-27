using Borigran.OneData.Domain.Entities;

namespace Borigran.OneData.Domain.NHibernate.Mapping
{
    public class ConstructorItemPositionMap: MapBase<ConstructorItemPosition>
    {
        public ConstructorItemPositionMap()
            :base()
        {
            Map(x => x.ImageName).Not.Nullable();
            Map(x => x.Position).Not.Nullable()
                .CustomType<int>();

            References(x => x.Item)
                .Not.Nullable()
                .Column("ConstructorItemId")
                .ForeignKey("FK_ConstructorItem_Position")
                .Index("IDX_ConstructorItemPosition_ConstructorItemId");
        }
    }
}
