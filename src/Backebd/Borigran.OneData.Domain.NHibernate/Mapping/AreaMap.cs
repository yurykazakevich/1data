using Borigran.OneData.Domain.Entities;

namespace Borigran.OneData.Domain.NHibernate.Mapping
{
    public class AreaMap : MapBase<Area>
    {
        public AreaMap()
            :base()
        {
            Map(x => x.Name).Not.Nullable();
            Map(x => x.Length).Not.Nullable();
            Map(x => x.Width).Not.Nullable();
            Map(x => x.BurialType).Not.Nullable()
                .CustomType<int>();

        }
    }
}
