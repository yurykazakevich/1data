using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Borigran.OneData.Platform.NHibernate.Transactions
{
    [DebuggerStepThrough]
    public class TransactionInterceptorSelector : IInterceptorSelector
    {
        public IInterceptor[] SelectInterceptors(Type type, System.Reflection.MethodInfo method, IInterceptor[] interceptors)
        {
            var interceptorList = new List<IInterceptor>(interceptors);
            IInterceptor transactionInterceptor =
                interceptorList.FirstOrDefault(x => x.GetType() == typeof(TransactionInterceptor));
            MethodInfo typeMethod = type.GetMethod(method.Name, 
                method.GetParameters().Select(x => x.ParameterType).ToArray());

            if (transactionInterceptor != null && typeMethod != null &&
                typeMethod.GetCustomAttribute<TransactionAttribute>() == null)
            {
                interceptorList.Remove(transactionInterceptor);
            }

            return interceptorList.ToArray();
        }
    }
}
