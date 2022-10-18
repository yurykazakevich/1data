using Borigran.OneData.Business.Dto;
using Borigran.OneData.Domain.Values;

namespace Borigran.OneData.Business
{
    public interface IConstructorService
    {
        Task<IEnumerable<ConstructorItemDto>> GetConstructorItemList(
            BurialTypes burialType, CItemTypes itemType);
    }
}