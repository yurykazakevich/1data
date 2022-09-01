using System;
using System.Data;

namespace Borigran.OneData.Platform.NHibernate.Transactions
{
    [AttributeUsage(AttributeTargets.Method)]
    public class TransactionAttribute : Attribute
    {
        private IsolationLevel isolationLevel = IsolationLevel.ReadCommitted;

        /// <summary>
        /// Sets the isolation level of the operation.
        /// </summary>
        /// <remarks>DO NOT Use this unless approved by architecture</remarks>
        public IsolationLevel IsolationLevel
        {
            get { return isolationLevel; }
            set { isolationLevel = value; }
        }

        /// <summary>
        /// Optional handler to hook into transaction lifecycle events
        /// Must implement the ITransactionLifecycleHandler interface
        /// </summary>
        public Type HandlerType { get; set; }
    }
}
