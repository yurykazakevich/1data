using Autofac;
using Borigran.OneData.Authorization.Jwt;

namespace Borigran.OneData.Authorization.Dependencies
{
    public class AuthorithationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AuthService>()
                .As<IAuthService>()
                .InstancePerLifetimeScope();
        }
    }
}
