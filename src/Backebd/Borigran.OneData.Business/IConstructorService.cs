using Borigran.OneData.Business.Dto;
using Borigran.OneData.Domain.Values;

namespace Borigran.OneData.Business
{
    public interface IConstructorService
    {
        Task<IEnumerable<ConstructorListItemDto>> GetConstructorItemList(CItemTypes itemType);
    }
}