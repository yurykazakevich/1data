using AutoMapper;
using Borigran.OneData.Business.Dto;
using Borigran.OneData.Domain.Entities;
using Borigran.OneData.Platform;

namespace Borigran.OneData.Business.AutoMapperProfiles
{
    public class ConstructorItemProfile : Profile
    {
        public ConstructorItemProfile(AppSettings appSettings)
        {
            CreateMap<ConstructorItemCategory, ConstructorItemCategoryDto>();
            CreateMap<ConstructorItemPosition, ConstructorItemPositionDto>();

            CreateMap<ConstructorItem, ConstructorItemDto>()
                .ForMember(dst => dst.Currency, opt => opt.MapFrom(src => appSettings.DefaultCurrency));
        }
    }
}
