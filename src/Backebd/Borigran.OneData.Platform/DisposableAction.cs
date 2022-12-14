using System;

namespace Borigran.OneData.Platform
{
    /// <summary>
    /// Better sytnax for context operation.
    /// Wraps a delegate that is executed when the Dispose method is called.
    /// This allows to do context sensitive things easily.
    /// Basically, it mimics Java's anonymous classes.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the parameter that the delegate to execute on dispose
    /// will accept
    /// </typeparam>
    public class DisposableAction<T> : IDisposable
    {
        readonly Action<T> _action;
        readonly T _val;
        private bool disposed = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="DisposableAction&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="action">The action to execute on dispose</param>
        /// <param name="val">The value that will be passed to the action on dispose</param>
        public DisposableAction(Action<T> action, T val)
        {
            if (action == null)
                throw new ArgumentNullException("action");
            _action = action;
            _val = val;
        }

        ~DisposableAction()
        {
            Dispose();
        }

        /// <summary>
        /// Gets the value associated with this action
        /// </summary>
        /// <value>The value.</value>
        public T Value
        {
            get { return _val; }
        }


        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing && !disposed)
            {
                _action(_val);
                disposed = true;
            }
        }
    }

    /// <summary>
    /// Better sytnax for context operation.
    /// Wraps a delegate that is executed when the Dispose method is called.
    /// This allows to do context sensitive things easily.
    /// Basically, it mimics Java's anonymous classes.
    /// </summary>
    public class DisposableAction : IDisposable
    {
        Action _action;

        /// <summary>
        /// Initializes a new instance of the <see cref="DisposableAction"/> class.
        /// </summary>
        /// <param name="action">The action to execute on dispose</param>
        public DisposableAction(Action action)
        {
            if (action == null)
                throw new ArgumentNullException("action");
            _action = action;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _action();
            }
        }
    }
}
