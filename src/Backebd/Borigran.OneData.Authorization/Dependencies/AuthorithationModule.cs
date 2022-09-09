using Autofac;
using Borigran.OneData.Authorization.Impl;

namespace Borigran.OneData.Authorization.Dependencies
{
    public class AuthorithationModule : Module
    {
        private readonly AuthOptions authOptions;

        public AuthorithationModule(AuthOptions authOptions)
        {
            this.authOptions = authOptions;
        }
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterInstance(authOptions).SingleInstance();

            builder.RegisterType<AuthService>()
                .As<IAuthService>()
                .InstancePerLifetimeScope();
        }
    }
}
