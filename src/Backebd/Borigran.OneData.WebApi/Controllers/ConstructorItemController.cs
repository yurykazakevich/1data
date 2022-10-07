using Borigran.OneData.Business;
using Borigran.OneData.WebApi.Models.Constructor;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Borigran.OneData.WebApi.Controllers
{
    public class ConstructorItemController : ApiControllerBase
    {
        private readonly IConstructorService constructorService;

        public ConstructorItemController(IConstructorService constructorService)
        {
            this.constructorService = constructorService;
        }

        [HttpGet]
        public async Task<IEnumerable<ListItemResponse>> Get([FromQuery]ItemListRequest request)
        {
            var dtoList = await constructorService.GetConstructorItemList(request.ItemType);
        }
    }
}
