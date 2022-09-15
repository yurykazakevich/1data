using Borigran.OneData.Authorization;
using Borigran.OneData.WebApi.Models.Developer;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Borigran.OneData.WebApi.Controllers
{
    public class DeveloperController : ApiControllerBase
    {
        private readonly IAuthService authService;

        public DeveloperController(IAuthService authService)
        {
            this.authService = authService;
        }

        [HttpGet("token")]
        public TokenResponse GetToken()
        {
            var claim = new Claim(ClaimTypes.Name, "developer");
            string token = authService.GenerateAccessToken(new Claim[] { claim });
            return new TokenResponse
            {
                Token = token,
                RefreshToken = "djkdsjkdhfjkshfjdhajkfhdakjskf"
            };
        }
    }
}
