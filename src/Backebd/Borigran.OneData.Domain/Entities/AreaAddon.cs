using Borigran.OneData.Domain.Values;
namespace Borigran.OneData.Domain.Entities
{
    public class AreaAddon : EntityBase
    {
        public virtual string Name { get; set; }
   
        public virtual AreaAddonTypes AddonType { get; set; }

        public virtual Area Area { get; set; }
    }
}
