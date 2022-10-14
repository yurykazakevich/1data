using AutoMapper;
using Borigran.OneData.Authorization;
using Borigran.OneData.Authorization.Dto;
using Borigran.OneData.WebApi.Models.Auth;
using Borigran.OneData.WebApi.Pipeline.RefreshToken;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Net;
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

            RefreshTokenCookieManager.AddCookie(Response, authToken.RefreshToken);
            return mapper.Map<TokenResponse>(authToken);
        }

        [HttpPatch("logout")]
        public async Task Logout([FromBody] UserIdRequest request)
        {
            await authService.LogoutAsync(request.UserId);
        }

        [AllowAnonymous]
        [HttpPatch("token/refresh")]
        public async Task<TokenResponse> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            string refreshToken = RefreshTokenCookieManager.GetToken(Request);

            if (string.IsNullOrEmpty(refreshToken))
            {
                throw new SecurityTokenException("Could not find refresh token cookie");
            }

            AuthTokenDto authToken = await authService.RefreshExpiredTokenAsync(request.ExpiredToken,
                refreshToken, request.UserId);

            RefreshTokenCookieManager.AddCookie(Response, authToken.RefreshToken);

            return mapper.Map<TokenResponse>(authToken);
        }
    }
}
