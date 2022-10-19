using Autofac;
using AutoMapper.Contrib.Autofac.DependencyInjection;
using Borigran.OneData.Platform.Http;
using Microsoft.Extensions.Configuration;
using Module = Autofac.Module;

namespace Borigran.OneData.Platform.Dependencies
{
    public class OneDataAutofacModule : Module
    {
        private readonly AssemblyScanner assemblyScanner = new AssemblyScanner();
        private readonly IConfiguration appConfig;

        public OneDataAutofacModule(IConfiguration appConfig)
        {
            this.appConfig = appConfig;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule(new NHibernateModule(appConfig));
            builder.RegisterModule<EncryptionModule>();

            builder.RegisterAutoMapper(false, assemblyScanner.ProjectAssemblies());

            builder.RegisterType<HttpHelper>().As<IHttpHelper>().SingleInstance();
        }

    }
}
