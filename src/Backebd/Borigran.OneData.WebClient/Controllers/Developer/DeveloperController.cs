using Borigran.OneData.Authorization;
using Borigran.OneData.WebClient.Controllers.Developer.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Borigran.OneData.WebClient.Controllers.Developer
{
    [Route("dev")]
    public class DeveloperController : ApiControllerBase
    {
        private readonly IAuthService authService;

        public DeveloperController(IAuthService authService)
        {
            this.authService = authService;
        }

        [HttpGet("token")]
        public IActionResult GetToken()
        {
            var claim = new Claim(ClaimTypes.Name, "developer");
            string token = authService.GenerateAccessToken(new Claim[] { claim });
            return Ok(new TokenResponse
            {
                Token = token,
                RefreshToken = "djkdsjkdhfjkshfjdhajkfhdakjskf"
            });
        }
    }
}
