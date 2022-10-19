using AutoMapper;
using Borigran.OneData.Business;
using Borigran.OneData.Platform.Helpers;
using Borigran.OneData.WebApi.Models.Constructor;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Borigran.OneData.WebApi.Controllers
{
    public class ConstructorItemController : ApiControllerBase
    {
        private readonly IConstructorService constructorService;
        private readonly ICItemImageProvider<Stream> imageProvider;
        private readonly IMapper mapper;

        public ConstructorItemController(IConstructorService constructorService,
            ICItemImageProvider<Stream> imageProvider,
            IMapper mapper)
        {
            this.constructorService = constructorService;
            this.imageProvider = imageProvider;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<ListItemResponse>> Get([FromQuery]ItemListRequest request)
        {
            var result = new List<ListItemResponse>();
            var dtoList = await constructorService.GetConstructorItemList(
                request.BurialType, request.ItemType);

            foreach(var dto in dtoList)
            {
                foreach (var position in dto.PossiblePositions)
                {
                    var responseItem = mapper.Map<ListItemResponse>(dto);
                    responseItem.Position = position.Position;
                    using (var imageStream = imageProvider.
                        GetItemImage(request.BurialType,
                        request.ItemType, responseItem.Categories, position.ImageName))
                    {
                        const int bufferSize = 1024;
                        var buffer = new byte[bufferSize];
                        var image = new List<byte>((int)imageStream.Length);
                        int readPosition = 0;

                        while (readPosition < imageStream.Length)
                        {
                            var read = imageStream.Read(buffer, readPosition, bufferSize);
                            readPosition += read;
                            image.AddRange(buffer);
                        }

                        responseItem.Image = Convert.ToBase64String(image.ToArray());
                    }
                    result.Add(responseItem);
                }
            }

            return result;
        }
    }
}
