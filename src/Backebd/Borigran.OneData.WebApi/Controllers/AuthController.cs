using AutoMapper;
using Borigran.OneData.Authorization;
using Borigran.OneData.Authorization.Dto;
using Borigran.OneData.WebApi.Models.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Borigran.OneData.WebApi.Controllers
{
    public class AuthController : ApiControllerBase
    {
        private readonly IMapper mapper;
        private readonly IAuthService authService;

        public AuthController(IMapper mapper, IAuthService authService)
        {
            this.mapper = mapper;
            this.authService = authService;
        }
    
        [HttpPost("sendsmscode")]
        [AllowAnonymous]
        public async Task<VerificationCodeResponse> GetVerificationCode([FromBody] PhoneNumberRequest request)
        {
            string code = await authService.GetVerificationCodeAsync(request.PhoneNumber);

            return new VerificationCodeResponse
            {
                Code = code
            };
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<TokenResponse> Login([FromBody] LoginRequest request)
        {
            var loginData = mapper.Map<LoginDto>(request);

            AuthTokenDto authToken = await authService.RegisterOrLoginAsync(loginData);

            return mapper.Map<TokenResponse>(authToken);
        }

        [HttpPatch("logout")]
        public async Task Logout([FromBody] PhoneNumberRequest request)
        {
            await authService.LogoutAsync(request.PhoneNumber);
        }

        [HttpPatch("token/refresh")]
        public async Task<TokenResponse> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            AuthTokenDto authToken = await authService.RefreshExpiredTokenAsync(request.ExpiredToken,
                request.RefreshToken, request.PhoneNumber);

            return mapper.Map<TokenResponse>(authToken);
        }
    }
}
