using AutoMapper;
using Borigran.OneData.Authorization.Dto;
using Borigran.OneData.WebApi.Models.Auth;

namespace Borigran.OneData.WebApi.Models.AutoMapperProfiles
{
    public class LoginRequestProfile : Profile
    {
        public LoginRequestProfile()
        {
            CreateMap<LoginRequest, LoginDto>();
        }
    }
}
