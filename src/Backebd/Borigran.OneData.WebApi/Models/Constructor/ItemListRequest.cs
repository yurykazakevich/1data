using Borigran.OneData.Domain.Values;

namespace Borigran.OneData.WebApi.Models.Constructor
{
    public class ItemListRequest
    {
        public BurialTypes BurialType { get; set; }
        public CItemTypes ItemType { get; set; }
    }
}
