using Borigran.OneData.Domain.Values;
using Borigran.OneData.Platform.Helpers;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace Borigran.OneData.WebApi.Logic
{
    public class CItemImageProvider : ICItemImageProvider<string>
    {
        public const string BacgroundImageName = "Фон.jpg";
        public const string NotFoundImageName = "notfound.png";
        private const string CItemFolderPath = "StaticResources\\citemimages";
        private const string CItemRootUrl = "resources/citemimages";

        private readonly ILogger<CItemImageProvider> logger;
        private readonly string imageRootFolderPath;
        public CItemImageProvider(ILogger<CItemImageProvider> logger)
        {
            this.logger = logger;
            string rootFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            imageRootFolderPath = Path.Combine(rootFolder, CItemFolderPath);
        }

        public string GetBacgroundImage(BurialTypes burialType)
        {
            string path = Path.Combine(imageRootFolderPath, burialType.ToString(), BacgroundImageName);

            path = ValidateImageFilePath(path);
            return PathToUrl(path);
        }

        public string GetItemImage(BurialTypes burialType, int itemId)
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

        private string PathToUrl(string path)
        {
            int startIndex = path.IndexOf(CItemFolderPath);
            var urlBuilder = new StringBuilder(path);
            urlBuilder.Remove(0, startIndex);
            urlBuilder.Replace(CItemFolderPath, CItemRootUrl);
            urlBuilder.Replace(Path.PathSeparator, '/');

            return urlBuilder.ToString();
            
        }
    }
}
