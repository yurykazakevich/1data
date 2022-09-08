using Borigran.OneData.Domain.Entities;

namespace Borigran.OneData.Domain.NHibernate.Mapping
{
    public class ImageMap : MapBase<Image>
    {
        public ImageMap()
            : base()
        {
            Map(x => x.ImageType).CustomType(typeof(int));
            Map(x => x.ImageData);
        }
    }
}
