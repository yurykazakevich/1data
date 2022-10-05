using Borigran.OneData.Domain.Values;
using Borigran.OneData.Platform.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Reflection;

namespace Borigran.OneData.WebApi.Logic
{
    public class CItemImageProvider : ICItemImageProvider<Stream>
    {
        public const string BacgroundImageName = "Фон.jpg";
        public const string NotFoundImageName = "notfound.png";
        private const string CItemFolderPath = "StaticResources\\citemimages";

        private readonly ILogger<CItemImageProvider> logger;
        private readonly string imageRootFolderPath;
        public CItemImageProvider(ILogger<CItemImageProvider> logger)
        {
            this.logger = logger;
            string rootFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            imageRootFolderPath = Path.Combine(rootFolder, CItemFolderPath);
        }

        public Stream GetBacgroundImage(BurialTypes burialType)
        {
            string path = Path.Combine(imageRootFolderPath, burialType.ToString(), BacgroundImageName);

            path = ValidateImageFilePath(path);
            return File.OpenRead(path);
        }

        public Stream GetItemImage(BurialTypes burialType, int itemId)
        {
            throw new NotImplementedException();
        }

        private string ValidateImageFilePath(string path)
        {
            if(!File.Exists(path))
            {
                logger.LogWarning($"Image not found! {path}");
                return Path.Combine(imageRootFolderPath, NotFoundImageName);
            }

            return path;
        }
    }
}
