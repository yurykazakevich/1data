using Autofac;
using Borigran.OneData.Authorization.Impl;
using Borigran.OneData.Platform.Dependencies;
using Borigran.OneData.Platform.Helpers;

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

            builder.RegisterType<SmsSender>()
                .As<ISmsSender>()
                .InstancePerLifetimeScope();

            builder.RegisterType<JwtTokenGenerator>()
                .As<ITokenGenerator>()
                .InstancePerLifetimeScope();

            builder.RegisterType<AuthService>()
                .As<IAuthService>()
                .EnableTransactionInterceptor()
                .InstancePerLifetimeScope();     
            
            builder.RegisterType<PhoneNumberHelper>()
                .As<IPhoneNumberHelper>()
                .InstancePerDependency();
        }
    }
}
