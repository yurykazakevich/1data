using Borigran.OneData.WebApi.Pipeline;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Borigran.OneData.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    [Authorize]
    [TypeFilter(typeof(FluentValidationFilter))]
    public abstract class ApiControllerBase : ControllerBase
    {
    }

}
