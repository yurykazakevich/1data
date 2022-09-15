using Autofac.Builder;
using Autofac.Extras.DynamicProxy;
using Borigran.OneData.Platform.NHibernate.Transactions;
using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Borigran.OneData.Platform.Dependencies
{
    public static class BuilderExtensions
    {
        public static IRegistrationBuilder<TLimit, TActivatorData, TSingleRegistrationStyle> EnableTransactionInterceptor<TLimit, TActivatorData, TSingleRegistrationStyle>(
        this IRegistrationBuilder<TLimit, TActivatorData, TSingleRegistrationStyle> registration)
        {
            return registration.EnableInterfaceInterceptors(new ProxyGenerationOptions
            {
                Selector = new TransactionInterceptorSelector()
            })
            .InterceptedBy(typeof(TransactionInterceptor));
        }
    }
}
