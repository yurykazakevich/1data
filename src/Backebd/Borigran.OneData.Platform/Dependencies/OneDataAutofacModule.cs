using Autofac;
using Autofac.Extras.DynamicProxy;
using Borigran.OneData.Platform.NHibernate.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Module = Autofac.Module;

namespace Borigran.OneData.Platform.Dependencies
{
    public class OneDataAutofacModule : Module
    {

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule<NHibernateModule>();
        }

    }
}
