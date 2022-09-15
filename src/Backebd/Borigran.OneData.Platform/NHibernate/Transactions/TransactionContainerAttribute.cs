using System;

namespace Borigran.OneData.Platform.NHibernate.Transactions
{
    [AttributeUsage(AttributeTargets.Class)]
    public class TransactionContainerAttribute : Attribute
    {
    }
}
