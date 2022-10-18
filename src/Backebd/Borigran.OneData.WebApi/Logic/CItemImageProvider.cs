using Borigran.OneData.Domain.Values;
using Borigran.OneData.Platform.Helpers;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Borigran.OneData.WebApi.Logic
{
    public class CItemImageProvider : ICItemImageProvider<Stream>
    {
        public const string CItemImageExtension = ".png";
        public const string BacgroundImageName = "Фон.png";
        public const string NotFoundImageName = "notfound.png";
        private const string CItemFolderPath = "StaticResources";

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

        public Stream GetItemImage(BurialTypes burialType, 
            CItemTypes itemType, IEnumerable<string> categoryNames,  string imageName)
        {
            var imagePath = GetItemImagePath(burialType, itemType, categoryNames, imageName);
            imagePath = ValidateImageFilePath(imagePath);

            return File.OpenRead(imagePath);
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

        private string GetItemImagePath(BurialTypes burialType,
            CItemTypes itemType, IEnumerable<string> categoryNames, string imageName)
        {
            var imagePathSegments = new List<string>() { CItemFolderPath };
            imagePathSegments.Add(burialType.ToString());
            imagePathSegments.Add("2.Гранитные комплектующие");

            imagePathSegments.AddRange(GetFolderForItemType(itemType));
            imagePathSegments.AddRange(categoryNames);
            imagePathSegments.Add(imageName + CItemImageExtension);

            return Path.Combine(imagePathSegments.ToArray());
        }

        private IEnumerable<string> GetFolderForItemType(CItemTypes itemType)
        {
            var result = new List<string>(2);

            if(itemType >= CItemTypes.Tip)
            {
                result.Add("6.Дополнения");
            }
            switch(itemType)
            {
                case CItemTypes.Pedestal:
                    result.Add("1.Тумбы");
                    break;
                case CItemTypes.Garden:
                    result.Add("2.Цветники");
                    break;
                case CItemTypes.Stele:
                    result.Add("3.Стелы");
                    break;
                case CItemTypes.Tombstone:
                    result.Add("4.Надгробные плиты");
                    break;
                case CItemTypes.Boder:
                    result.Add("5.Ограды");
                    break;
                case CItemTypes.Tip:
                    result.Add("1.Пики, шары");
                    break;
                case CItemTypes.Bench:
                    result.Add("2.Вазы");
                    break;
                case CItemTypes.Vase:
                    result.Add("3.Лампады");
                    break;
                case CItemTypes.Lampada:
                    result.Add("4.Лавки");
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(itemType), $"Unknown item type {itemType}");
            }

            return result;
        }
    }
}
