using Borigran.OneData.Domain.Values;

namespace Borigran.OneData.Platform.Helpers
{
    public interface ICItemImageProvider<TImageViewType>
    {
        public TImageViewType GetItemImage(BurialTypes burialType, int itemId);

        public TImageViewType GetBacgroundImage(BurialTypes burialType);
    }
}
