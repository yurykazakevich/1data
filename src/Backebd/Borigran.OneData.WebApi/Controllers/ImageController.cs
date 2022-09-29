using Borigran.OneData.Domain.Values;
using Borigran.OneData.Platform.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

namespace Borigran.OneData.WebApi.Controllers
{
    public class ImageController : ApiControllerBase
    {
        private readonly ICItemImageProvider<Stream> imageProvider;

        public ImageController(ICItemImageProvider<Stream> imageProvider)
        {
            this.imageProvider = imageProvider;
        }

        [HttpGet("bacground")]
        public FileResult GetBacgroundImage(BurialTypes burialType)
        {
            return File(imageProvider.GetBacgroundImage(burialType), "image/png");
        }
    }
}
