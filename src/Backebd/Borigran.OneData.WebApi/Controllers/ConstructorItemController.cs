using Borigran.OneData.Business;
using Borigran.OneData.Platform.Helpers;
using Borigran.OneData.WebApi.Models.Constructor;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Borigran.OneData.WebApi.Controllers
{
    public class ConstructorItemController : ApiControllerBase
    {
        private readonly IConstructorService constructorService;
        private readonly ICItemImageProvider<string> imageProvider;

        public ConstructorItemController(IConstructorService constructorService,
            ICItemImageProvider<string> imageProvider)
        {
            this.constructorService = constructorService;
            this.imageProvider = imageProvider;
        }

        /*[HttpGet]
        public async Task<IEnumerable<ListItemResponse>> Get([FromQuery]ItemListRequest request)
        {
            var dtoList = await constructorService.GetConstructorItemList(request.ItemType);
        }*/
    }
}
