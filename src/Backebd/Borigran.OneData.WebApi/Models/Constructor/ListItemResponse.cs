using Borigran.OneData.Domain.Values;

namespace Borigran.OneData.WebApi.Models.Constructor
{
    public class ListItemResponse
    {
        public int ItemId { get; set; }
        public CItemTypes ItemType { get; set; }
        public string Name { get; set; }
        public string Size { get; set; }
        public decimal Price { get; set; }
    }
}
