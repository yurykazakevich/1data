using Borigran.OneData.Domain.Values;
using System.Collections.Generic;

namespace Borigran.OneData.WebApi.Models.Constructor
{
    public class ListItemResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Size { get; set; }
        public decimal Price { get; set; }
        public ItemPositions Position { get; set; }
        public string Image { get; set; }
        public IList<string> Categories { get; set; }
    }
}
