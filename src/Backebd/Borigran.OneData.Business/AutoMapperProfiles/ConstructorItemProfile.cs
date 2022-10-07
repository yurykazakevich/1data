using AutoMapper;
using Borigran.OneData.Business.Dto;
using Borigran.OneData.Domain.Entities;
using System.Text;

namespace Borigran.OneData.Business.AutoMapperProfiles
{
    public class ConstructorItemProfile : Profile
    {
        public ConstructorItemProfile()
        {
            CreateMap<ConstructorItem, ConstructorListItemDto>()
                .ForMember(dst => dst.Size, opt => opt.MapFrom(src => SizeToString(src)))
                .ForMember(dst => dst.ImageName, opt => opt.Ignore())
                .ForMember(dst => dst.Position, opt => opt.Ignore());
        }

        private string SizeToString(ConstructorItem src)
        {
            const string sizeSeparator = "x";

            var sizeBuilder = new StringBuilder(src.Length);
            if(src.Width > 0)
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

    public static class ConstructorItemMapperExtension
    {
        public static IEnumerable<ConstructorListItemDto> MapConstructorItemList(this IMapper mapper, IEnumerable<ConstructorItem> src)
        {
            var result = new List<ConstructorListItemDto>();
            foreach(var item in src)
            {
                result.AddRange(mapper.MapToListItem(item));
            }

            return result;
        }

        private static IEnumerable<ConstructorListItemDto> MapToListItem(this IMapper mapper, ConstructorItem src)
        {
            var result = new List<ConstructorListItemDto>();
            foreach (var position in src.PossiblePositions)
            {
                var listItem = mapper.Map<ConstructorItem, ConstructorListItemDto>(src, opt =>
                opt.AfterMap((src, dest) =>
                {
                    dest.ImageName = position.ImageName;
                    dest.Position = position.Position;
                }));

                result.Add(listItem);
            }

            return result;
        }
    }
}
