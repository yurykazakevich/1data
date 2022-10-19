using Borigran.OneData.Domain.Values;
using System.Collections.Generic;

namespace Borigran.OneData.Platform.Helpers
{
    public interface ICItemImageProvider<TImageViewType>
    {
        public TImageViewType GetItemImage(BurialTypes burialType, BurialPositions burialPosition,
            CItemTypes itemType, IEnumerable<string> categoryNames, string imageName);

        public TImageViewType GetBacgroundImage(BurialTypes burialType);
    }
}
