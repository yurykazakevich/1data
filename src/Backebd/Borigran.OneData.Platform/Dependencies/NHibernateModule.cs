using Autofac;
using Autofac.Extras.DynamicProxy;
using Borigran.OneData.Platform.NHibernate.Repository;
using Borigran.OneData.Platform.NHibernate.Transactions;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Borigran.OneData.Domain.NHibernate.Mapping;
using Configuration = NHibernate.Cfg.Configuration;
using Module = Autofac.Module;
using Environment = NHibernate.Cfg.Environment;
using IInterceptor = Castle.DynamicProxy.IInterceptor;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Borigran.OneData.Platform.Dependencies
{
    public class NHibernateModule : Module
    {
        private readonly AssemblyScanner assemblyScanner = new AssemblyScanner();

        private readonly string dbConnectionString;

        public NHibernateModule(IConfiguration appConfig)
        {
            dbConnectionString = appConfig.GetConnectionString("DefaultConnection");
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterGeneric(typeof(NHibernateRepository<>))
                .As(typeof(IRepository<>))
                .InstancePerLifetimeScope();

            builder.RegisterType<TransactionInterceptor>()
                .AsSelf()
                .InstancePerLifetimeScope();

            var cfg = BuildConfiguration();

            //var persistenceModel = BuildPersistenceModel();
            //persistenceModel.Configure(cfg);

            var sessionFactory = BuildSessionFactory(cfg);

            RegisterConponents(builder, cfg, sessionFactory);
        }

        public Configuration BuildConfiguration()
        {
            var config = Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2012.ConnectionString(dbConnectionString))
                .ExposeConfiguration(c => c.SetProperty(Environment.ReleaseConnections, "on_close"))
                .ExposeConfiguration(c => c.SetProperty(Environment.Hbm2ddlAuto, "create"))
                .ExposeConfiguration(c => c.SetProperty(Environment.ShowSql, "true"))
                .Mappings(m =>
                {
                    foreach (Assembly assembly in assemblyScanner.AssembliesToScan())
                    {
                        m.FluentMappings.AddFromAssembly(assembly);
                    }
                })
                .ExposeConfiguration(BuildSchema)
                .BuildConfiguration();

            if (config == null)
                throw new Exception("Cannot build NHibernate configuration");

            return config;
        }

        public ISessionFactory BuildSessionFactory(Configuration config)
        {
            var sessionFactory = config.BuildSessionFactory();

            if (sessionFactory == null)
                throw new Exception("Cannot build NHibernate Session Factory");

            return sessionFactory;
        }

        public void RegisterConponents(ContainerBuilder builder, Configuration config, ISessionFactory sessionFactory)
        {
            builder.RegisterInstance(config).As<Configuration>().SingleInstance();
            builder.RegisterInstance(sessionFactory).As<ISessionFactory>().SingleInstance();
            builder.Register(x => x.Resolve<ISessionFactory>().OpenSession()).As<ISession>().InstancePerLifetimeScope();
        }

        private static void BuildSchema(Configuration config)
        {
#if DEBUG
            var update = new SchemaUpdate(config);
            update.Execute(false, true);
#endif

            new SchemaExport(config).SetOutputFile(@"./Schema.sql").Create(true, true);
        }
    }
}
