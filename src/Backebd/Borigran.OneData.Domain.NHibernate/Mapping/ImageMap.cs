using Borigran.OneData.Domain.Entities;

namespace Borigran.OneData.Domain.NHibernate.Mapping
{
    public class ImageMap : MapBase<Image>
    {
        public ImageMap()
            : base()
        {
            Map(x => x.ImageType);
            Map(x => x.ImageData);
        }
    }
}
