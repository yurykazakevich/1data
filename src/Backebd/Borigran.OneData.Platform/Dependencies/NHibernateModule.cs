using Autofac;
using Borigran.OneData.Platform.NHibernate.Repository;

namespace Borigran.OneData.Platform.Dependencies
{
    public class NHibernateModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterGeneric(typeof(NHibernateRepository<>))
                .As(typeof(IRepository<>))
                .InstancePerLifetimeScope();
        }
    }
}
