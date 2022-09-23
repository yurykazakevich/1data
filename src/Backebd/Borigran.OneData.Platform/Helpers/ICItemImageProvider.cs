using Borigran.OneData.Domain.Values;

namespace Borigran.OneData.Platform.Helpers
{
    public interface ICItemImageProvider
    {
        public string GetImageUrl(BurialTypes imageType, params object[] imageSearchParams);
    }
}
