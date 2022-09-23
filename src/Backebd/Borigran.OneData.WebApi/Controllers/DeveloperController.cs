using Borigran.OneData.Authorization;
using Borigran.OneData.WebApi.Models.Auth;
using Microsoft.AspNetCore.Authorization;

namespace Borigran.OneData.WebApi.Controllers
{
    [AllowAnonymous]
    public class DeveloperController : ApiControllerBase
    {
        private readonly IAuthService authService;

        public DeveloperController(IAuthService authService)
        {
            this.authService = authService;
        }
    }
}
