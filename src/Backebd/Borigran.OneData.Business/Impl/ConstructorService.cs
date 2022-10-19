using AutoMapper;
using Borigran.OneData.Business.Dto;
using Borigran.OneData.Domain.Entities;
using Borigran.OneData.Domain.Values;
using Borigran.OneData.Platform.NHibernate.Repository;
using Borigran.OneData.Platform.NHibernate.Transactions;
using NHibernate.Linq;

namespace Borigran.OneData.Business.Impl
{
    public class ConstructorService : IConstructorService
    {
        private readonly IRepository<ConstructorItem> constructorItemRepository;
        private readonly IMapper mapper;

        public ConstructorService(IMapper mapper,
            IRepository<ConstructorItem> constructorItemRepository)
        {
            this.mapper = mapper;
            this.constructorItemRepository = constructorItemRepository;
        }

        public async Task<IEnumerable<ConstructorItemDto>> GetConstructorItemList(
            BurialTypes burialType, CItemTypes itemType)
        {
            var items = await constructorItemRepository.Query()
                .Where(x => x.ItemType == itemType)
                .Where(x => x.AllowedBurialTypes == null || 
                    x.AllowedBurialTypes.Contains(((int)burialType).ToString()))
                .ToListAsync();

            var dtoList = mapper.Map<IEnumerable<ConstructorItemDto>>(items);

            return dtoList;
        }
    }
}
