using Borigran.OneData.Domain.Values;

namespace Borigran.OneData.Business.Dto
{
    public class ConstructorListItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Size { get; set; }
        public string ImageName { get; set; }

        public ItemPositions Position { get; set; }
    }
}
