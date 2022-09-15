using Borigran.OneData.Authorization;
using Borigran.OneData.WebApi.Models.Developer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using System.Security.Claims;

namespace Borigran.OneData.WebApi.Controllers
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
        public ActionResult<TokenResponse> GetToken()
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
