using Autofac;
using Borigran.OneData.Business.Impl;
using Borigran.OneData.Platform.Dependencies;
using Borigran.OneData.Platform.Helpers;

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
