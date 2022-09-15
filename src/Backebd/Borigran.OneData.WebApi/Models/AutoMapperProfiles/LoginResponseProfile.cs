using AutoMapper;
using Borigran.OneData.Authorization.Dto;
using Borigran.OneData.WebApi.Models.Auth;

namespace Borigran.OneData.WebApi.Models.AutoMapperProfiles
{
    public class LoginResponseProfile : Profile
    {
        public LoginResponseProfile()
        {
            CreateMap<AuthTokenDto, TokenResponse>();
        }
    }
}
