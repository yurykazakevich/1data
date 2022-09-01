using Castle.Core.Interceptor;
using log4net;
using Microsoft.Extensions.Logging;
using NHibernate;
using NHibernate.Transaction;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Borigran.OneData.Platform.NHibernate.Transactions
{
    [DebuggerStepThrough]
    public class TransactionInterceptor : BaseInterceptor<TransactionInterceptor.TransactionState>, ISynchronization
    {
        private IInvocation currentInvocation;
        private readonly ISession session;
        private readonly ILogger<TransactionInterceptor> logger;
        private readonly IList<Action<bool>> transactionCallbacks = new List<Action<bool>>();
        private bool sync;

        public TransactionInterceptor(ISession session, ILogger<TransactionInterceptor> logger)
        {
            this.session = session;
            this.logger = logger;
        }

        public class TransactionState
        {
            public ITransaction Transaction { get; set; }
        }

        /// <summary>
        /// Allow handlers to register callbacks
        /// </summary>
        /// <param name="transactionCompleted"></param>
        public void RegisterCallback(Action<bool> transactionCompleted)
        {
            transactionCallbacks.Add(transactionCompleted);

            if (session.Transaction != null && session.Transaction.IsActive)
            {
                RegisterCallback(session.Transaction);
            }
            else
            {
                throw new InvalidOperationException("Cannot attach as no ambient transaction");
            }
        }

        /// <summary>
        /// Called before the intercepted method
        /// </summary>
        /// <param name="invocation"></param>
        /// <returns></returns>
        public override TransactionState BeforeAction(IInvocation invocation)
        {
            currentInvocation = invocation;
            var atts = invocation.MethodInvocationTarget.GetCustomAttributes(typeof(TransactionAttribute), true);
            var transactionAttribute = atts.Any() ? atts[0] as TransactionAttribute : null;

            if (transactionAttribute == null) return null;

            var isolationLevel = transactionAttribute.IsolationLevel;
            var methodName = invocation.Method.Name;

            if (session.Transaction != null && session.Transaction.IsActive)
            {
                RegisterCallback(session.Transaction);
                logger.LogDebug("Already in active transaction, so passthrough ({0})", methodName);
                return null;
            }

            logger.LogDebug("Starting new transaction ({0})", methodName);

            var tx = session.BeginTransaction(isolationLevel);
            RegisterCallback(tx);

            return new TransactionState { Transaction = tx };
        }


        /// <summary>
        /// Called after the intercepted method has completed
        /// </summary>
        /// <param name="invocation"></param>
        /// <param name="state"></param>
        public override void AfterAction(IInvocation invocation, TransactionState state)
        {
        }

        /// <summary>
        /// Called if the operation has run successfully
        /// </summary>
        /// <param name="invocation"></param>
        /// <param name="state"></param>
        public override void OnSuccessAction(IInvocation invocation, TransactionState state)
        {
            if (state == null) return;

            var methodName = invocation.Method.Name;
            var tx = state.Transaction;
            logger.LogDebug("Committing transaction ({0})", methodName);
            try
            {
                tx.Commit();
            }
            finally
            {
                tx.Dispose();
            }
        }


        /// <summary>
        /// Called if the operation has failed with an exception
        /// </summary>
        /// <param name="invocation"></param>
        /// <param name="ex"></param>
        /// <param name="state"></param>
        public override void OnFailureAction(IInvocation invocation, Exception ex, TransactionState state)
        {
            if (state == null) return;

            var methodName = invocation.Method.Name;
            var tx = state.Transaction;

            // This is not necessarily an error. Some exceptions will be returned as 400 
            // or as 403 responses, others as 500. Log this as debug info message only.
            // Global logging handler will log this exception if this results in 5xx error
            logger.LogInformation("Transaction rolling back ({0})", ex, methodName);
            try
            {
                if (tx.IsActive)
                    tx.Rollback();
            }
            finally
            {
                tx.Dispose();
            }
        }

        /// <summary>
        /// Callback from tx.RegisterSynchronization()
        /// </summary>
        public void BeforeCompletion() { }

        /// <summary>
        /// Callback from tx.RegisterSynchronization()
        /// </summary>
        public void AfterCompletion(bool success)
        {
            foreach (var callback in transactionCallbacks)
            {
                callback(success);
            }
        }

        private void RegisterCallback(ITransaction transaction)
        {
            if (sync) return;
            transaction.RegisterSynchronization(this);
            sync = true;
        }
    }
}
