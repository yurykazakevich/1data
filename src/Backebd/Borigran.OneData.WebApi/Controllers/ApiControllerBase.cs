using Microsoft.AspNetCore.Mvc;

namespace Borigran.OneData.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public abstract class ApiControllerBase : ControllerBase
    {
    }
}
