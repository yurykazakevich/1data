using AutoMapper;
using Borigran.OneData.Business.Dto;
using Borigran.OneData.Domain.Entities;

namespace Borigran.OneData.Business.AutoMapperProfiles
{
    public class ConstructorItemProfile : Profile
    {
        public ConstructorItemProfile()
        {
            CreateMap<ConstructorItemCategory, ConstructorItemCategoryDto>();
            CreateMap<ConstructorItemPosition, ConstructorItemPositionDto>();

            CreateMap<ConstructorItem, ConstructorItemDto>();
        }
    }
}
