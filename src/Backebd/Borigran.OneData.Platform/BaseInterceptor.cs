using Castle.DynamicProxy;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Borigran.OneData.Platform
{
    [DebuggerStepThrough]
    public abstract class BaseInterceptor<TState> : IInterceptor where TState : class
    {
        public void Intercept(IInvocation invocation)
        {
            if (invocation == null)
                throw new ArgumentNullException(nameof(invocation));

            var state = BeforeAction(invocation);

            try
            {
                invocation.Proceed();
                if (state == null)
                {
                    return;
                }

                if (invocation.ReturnValue is Task)
                {
                    // Regarding the implementation itself and disabled analyzers (VSTHRD003).
                    // Castle.DynamicProxy support of `async` operations is not that straightforward.
                    // There is no clear way at the moment how to satisfy the analyzer needs so
                    // we assume that the execution context won't change between the actual method call
                    // and the interceptor. Based on this assumption there should be no dead-lock produced
                    // by the microservice code.
                    InterceptAsync((dynamic)invocation.ReturnValue, invocation, state);
                }
                else
                {
                    InterceptSync(invocation, state);
                }
            }
            catch (Exception ex)
            {
                OnFailure(invocation, ex, state);
                throw;
            }
        }

        private void InterceptSync(IInvocation invocation, TState state) => OnSuccess(invocation, state);

        private void InterceptAsync(Task task, IInvocation invocation, TState state)
            => invocation.ReturnValue = DoInterceptAsync(task, invocation, state);

        private void InterceptAsync<T>(Task<T> task, IInvocation invocation, TState state)
            => invocation.ReturnValue = DoInterceptAsync(task, invocation, state);

        private async Task DoInterceptAsync(Task operation, IInvocation invocation, TState state)
        {
            try
            {
#pragma warning disable VSTHRD003
                await operation;
#pragma warning restore VSTHRD003

                OnSuccessAsync(invocation, state);
            }
            catch (Exception ex)
            {
                OnFailureAsync(invocation, ex, state);
                throw;
            }
        }

        private async Task<T> DoInterceptAsync<T>(Task<T> operation, IInvocation invocation, TState state)
        {
            try
            {
#pragma warning disable VSTHRD003
                var result = await operation;
#pragma warning restore VSTHRD003

                OnSuccessAsync(invocation, state);

                return result;
            }
            catch (Exception ex)
            {
                OnFailureAsync(invocation, ex, state);
                throw;
            }
        }

        public abstract TState BeforeAction(IInvocation invocation);
        public abstract void AfterAction(IInvocation invocation, TState state);
        public abstract void OnSuccessAction(IInvocation invocation, TState state);
        public abstract void OnFailureAction(IInvocation invocation, Exception exception, TState state);
        public abstract Task OnSuccessActionAsync(IInvocation invocation, TState state);
        public abstract Task OnFailureActionAsync(IInvocation invocation, Exception exception, TState state);

        private void OnSuccess(IInvocation invocation, TState state)
        {
            try
            {
                AfterAction(invocation, state);
                OnSuccessAction(invocation, state);
            }
            catch (Exception ex)
            {
                OnFailureAction(invocation, ex, state);
                throw;
            }
        }

        private async Task OnSuccessAsync(IInvocation invocation, TState state)
        {
            try
            {
                AfterAction(invocation, state);
                await OnSuccessActionAsync(invocation, state);
            }
            catch (Exception ex)
            {
                await OnFailureActionAsync(invocation, ex, state);
                throw;
            }
        }
        private void OnFailure(IInvocation invocation, Exception ex, TState state) => 
            OnFailureAction(invocation, ex, state);

        private async Task OnFailureAsync(IInvocation invocation, Exception ex, TState state) =>
            await OnFailureActionAsync(invocation, ex, state);
    }
}
