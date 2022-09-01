using Autofac;
using Autofac.Extras.DynamicProxy;
using Borigran.OneData.Platform.NHibernate.Repository;
using Borigran.OneData.Platform.NHibernate.Transactions;
using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Module = Autofac.Module;

namespace Borigran.OneData.Platform.Dependencies
{
    public class NHibernateModule : Module
    {
        private readonly AssemblyScanner assemblyScanner = new AssemblyScanner();

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterGeneric(typeof(NHibernateRepository<>))
                .As(typeof(IRepository<>))
                .InstancePerLifetimeScope();

            foreach (Type transactionContainer in GetTransactionContainerTypes())
            {
                builder.RegisterType(transactionContainer)
                    .EnableInterfaceInterceptors()
                    .InterceptedBy(typeof(TransactionInterceptor));
            }

            builder.RegisterType<TransactionInterceptor>()
                .AsSelf()
                .AsImplementedInterfaces()
                .InstancePerDependency()
                .Named<IInterceptor>(GlobalStrings.DbTransactionInterceptorName);
        }


        private Type[] GetTransactionContainerTypes()
        {
            var types = new List<Type>();

            foreach (Assembly assembly in assemblyScanner.AssembliesToScan())
            {
                types.AddRange(assembly.GetTypes()
                    .Where(t => t.GetCustomAttribute<InterceptAttribute>() != null));
            }

            return types.ToArray(); ;
        }
    }
}
