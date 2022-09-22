using Borigran.OneData.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Borigran.OneData.Domain.NHibernate.Mapping
{
    public class ConstructorItemImageMap : MapBase<ConstructorItemImage>
    {
        public ConstructorItemImageMap()
           : base()
        {
            Map(x => x.ImagePath).Not.Nullable();
            References(x => x.Item)
                .Column("ConstructorItemId")
                .Not.Nullable();
        }
    }
}
