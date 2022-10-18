using AutoMapper;
using Borigran.OneData.Business.Dto;
using Borigran.OneData.Domain.Entities;
using Borigran.OneData.WebApi.Models.Constructor;
using System.Collections.Generic;
using System.Text;

namespace Borigran.OneData.WebApi.Models.AutoMapperProfiles
{
    public class ConstructorItemProfile : Profile
    {
        public ConstructorItemProfile()
        {
            CreateMap<ConstructorItemDto, ListItemResponse>()
                .ForMember(dst => dst.Image, opt => opt.Ignore())
                .ForMember(dst => dst.Position, opt => opt.Ignore())
                .ForMember(dst => dst.Size, opt => opt.MapFrom(src => SizeToString(src)))
                .ForMember(dst => dst.Categories, opt => opt.MapFrom(src => MapCategories(src)));
        }

        private IList<string> MapCategories(ConstructorItemDto src)
        {
            var result = new List<string>();
            var category = src.Category;

            while(category != null)
            {
                result.Insert(0, category.Name);
                category = category.ParentCategory;
            }

            return result;
        }

        private string SizeToString(ConstructorItemDto src)
        {
            const string sizeSeparator = "x";

            var sizeBuilder = new StringBuilder(src.Length);
            if (src.Width > 0)
            {
                sizeBuilder.Append(sizeSeparator);
                sizeBuilder.Append(src.Width);
            }
            if (src.Height > 0)
            {
                sizeBuilder.Append(sizeSeparator);
                sizeBuilder.Append(src.Height);
            }

            return sizeBuilder.ToString();
        }
    }
}
