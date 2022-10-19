using Autofac;
using Borigran.OneData.Business.Impl;
using Borigran.OneData.Platform.Dependencies;

namespace Borigran.OneData.Business.Dependencies
{
    public class BusinessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ConstructorService>()
                .As<IConstructorService>()
                .InstancePerLifetimeScope();
        }
    }
}
