using Borigran.OneData.Domain.Values;
using Borigran.OneData.Platform.Helpers;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace Borigran.OneData.WebApi.Logic
{
    public class CItemImageProvider : ICItemImageProvider, IDisposable
    {
        public const string imageFileNameFormat = "{0}_{1}.png";

        private readonly string imageRootFolderPath;

        private Stream imageStream;

        public CItemImageProvider(IConfiguration appSettings)
        {
            imageRootFolderPath = appSettings.GetValue<string>("CItemImageRootFolder");
        }

        public void Dispose()
        {
            if (imageStream != null)
            {
                imageStream.Dispose();
            }
        }

        public string GetImageUrl(BurialTypes imageType, params object[] imageSearchParams)
        {
            string itemName;
            CItemTypes itemType;

            ParseParameters(imageSearchParams, out itemName, out itemType);

            string filePath = Path.Combine(imageRootFolderPath, itemType.ToString(),
                string.Format(imageFileNameFormat, itemName, (int)imageType));

            return filePath.Replace(Path.PathSeparator, '/');
        }

        private void ParseParameters(object[] imageSearchParams, out string itemName, out CItemTypes itemType)
        {
            if (imageSearchParams.Length < 2)
            {
                throw new ArgumentException("ItemName and ItemType were expected in imageSearchParams", nameof(imageSearchParams));
            }

            itemName = imageSearchParams[0] as string;
            if (string.IsNullOrEmpty(itemName))
            {
                throw new ArgumentException("String ItemName was expected in the first item of imageSearchParams", nameof(imageSearchParams));
            }

            if (!(imageSearchParams[1] is CItemTypes))
            {
                throw new ArgumentException("CItemTypes ItemType was expected in the second item of imageSearchParams", nameof(imageSearchParams));
            }

            itemType = (CItemTypes)imageSearchParams[1];
        }
    }
}
