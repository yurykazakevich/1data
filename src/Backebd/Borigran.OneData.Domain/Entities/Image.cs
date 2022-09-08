using Borigran.OneData.Domain.Values;

namespace Borigran.OneData.Domain.Entities
{
    public class Image : EntityBase
    {
        public virtual ImageTypes ImageType { get; set; }

        public virtual byte[] ImageData { get; set; }
    }
}
