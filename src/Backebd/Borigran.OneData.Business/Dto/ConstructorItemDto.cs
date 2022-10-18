using Borigran.OneData.Domain.Values;

namespace Borigran.OneData.Business.Dto
{
    public class ConstructorItemCategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int SortOrder { get; set; }

        public ConstructorItemCategoryDto ParentCategory { get; set; }
    }

    public class ConstructorItemPositionDto
    {
        public string ImageName { get; set; }

        public ItemPositions Position { get; set; }
    }
    public class ConstructorItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string ArticleNumber { get; set; }
        public string Material { get; set; }
        public int Length { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public decimal Weight { get; set; }
        public int Varranty { get; set; }
        public ConstructorItemCategoryDto? Category { get; set; }
        public IEnumerable<ConstructorItemPositionDto> PossiblePositions { get; set; }
    }
}
