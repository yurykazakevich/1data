using Borigran.OneData.Domain.Values;
using Borigran.OneData.Platform.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace Borigran.OneData.WebApi.Controllers
{
    public class ImageController : ApiControllerBase
    {
        private readonly ICItemImageProvider<Stream> imageProvider;

        public ImageController(ICItemImageProvider<Stream> imageProvider)
        {
            this.imageProvider = imageProvider;
        }

        [HttpGet("background")]
        public FileStreamResult GetBacgroundImage([FromQuery]BurialTypes burialType)
        {
            return File(imageProvider.GetBacgroundImage(burialType), WebApiConst.ImageContentType);
        }                
    }
}
