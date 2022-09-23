using Borigran.OneData.Domain.Entities;

namespace Borigran.OneData.Domain.NHibernate.Mapping
{
    public class AreaAddonMap : MapBase<AreaAddon>
    {
        public AreaAddonMap()
            :base()
        {
            Map(x => x.Name).Not.Nullable();
            Map(x => x.AddonType).Not.Nullable()
                .CustomType<int>();

            References(x => x.Area)
                .Column("AreaId")
                .ForeignKey("FK_Area_AreaAddon")
                .Index("IDX_AreaAddon_AreaId")
                .Not.LazyLoad();
        }
    }
}
