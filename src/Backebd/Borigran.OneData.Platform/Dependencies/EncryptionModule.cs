using Autofac;
using Borigran.OneData.Platform.Encryption;

namespace Borigran.OneData.Platform.Dependencies
{
    public class EncryptionModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<HashEncryptor>()
                .As<IHashEncryptor>()
                .InstancePerLifetimeScope();
        }
    }
}
