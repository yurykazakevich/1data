﻿using Borigran.OneData.Domain.Entities;

namespace Borigran.OneData.Domain.NHibernate.Mapping
{
    public class ConstructorItemMap : MapBase<ConstructorItem>
    {
        public ConstructorItemMap()
            :base()
        {
            Map(x => x.Name).Not.Nullable();
            Map(x => x.Price).Not.Nullable();
            Map(x => x.ArticleNumber).Not.Nullable();
            Map(x => x.Material).Not.Nullable();
            Map(x => x.Length).Not.Nullable();
            Map(x => x.Width).Not.Nullable();
            Map(x => x.Height).Not.Nullable();
            Map(x => x.Weight).Not.Nullable();
            Map(x => x.Varranty).Not.Nullable();

            HasMany(x => x.Images)
                .Cascade.Delete()
                .Not.LazyLoad()
                .ForeignKeyConstraintName("FK_ConstructorItem_Image");
        }
    }
}
