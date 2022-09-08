using FluentNHibernate.Mapping;

namespace Borigran.OneData.Domain.NHibernate.Mapping
{
    public class MapBase<TEntity> : ClassMap<TEntity>
        where TEntity : EntityBase
    {
        public MapBase()
        {
            string entityName = typeof(TEntity).Name;

            Id(item => item.Id)
                .Column($"{entityName}ID")
                .GeneratedBy.Identity();

            Table($"T{entityName}s");
        }
    }
}
