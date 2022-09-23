using Borigran.OneData.Domain.Values;

namespace Borigran.OneData.Domain.Entities
{
    public class Area : EntityBase
    {
        public virtual string Name { get; set; }
        public virtual int Length { get; set; }
        public virtual int Width { get; set; }
        public virtual BurialTypes BurialType { get; set; }
    }
}
