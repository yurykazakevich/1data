using Borigran.OneData.Domain.Values;
using Borigran.OneData.Platform.Helpers;
using Borigran.OneData.WebApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace Borigran.OneData.WebApi.Controllers
{
    public class ImageController : ApiControllerBase
    {
        private readonly ICItemImageProvider<string> imageProvider;

        public ImageController(ICItemImageProvider<string> imageProvider)
        {
            this.imageProvider = imageProvider;
        }

        [HttpGet("background")]
        public ImageUrlResponse GetBacgroundImage(BurialTypes burialType)
        {
            return new ImageUrlResponse
            {
                ImageUrl = imageProvider.GetBacgroundImage(burialType)
            };
        }                
    }
}
