using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Borigran.OneData.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    [Authorize]
    public abstract class ApiControllerBase : ControllerBase
    {
    }
}
