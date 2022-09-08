using Borigran.OneData.Domain.Entities;

namespace Borigran.OneData.Domain.NHibernate.Mapping
{
    public class ConstructorItemMap : MapBase<ConstructorItem>
    {
        public ConstructorItemMap()
            :base()
        {
            Map(x => x.ImageId);
        }
    }
}
