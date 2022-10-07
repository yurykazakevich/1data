using AutoMapper;
using Borigran.OneData.Business.AutoMapperProfiles;
using Borigran.OneData.Business.Dto;
using Borigran.OneData.Domain.Entities;
using Borigran.OneData.Domain.Values;
using Borigran.OneData.Platform.NHibernate.Repository;
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

        public async Task<IEnumerable<ConstructorListItemDto>> GetConstructorItemList(CItemTypes itemType)
        {
            var items = await constructorItemRepository.Query()
                .Where(x => x.ItemType == itemType)
                .ToListAsync();

            var dtoList = mapper.MapConstructorItemList(items);

            return dtoList;
        }
    }
}
