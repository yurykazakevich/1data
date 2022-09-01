using Castle.DynamicProxy;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Borigran.OneData.Platform
{
    public abstract class BaseInterceptor<TState> : IInterceptor where TState : class
    {
        private static readonly MethodInfo interceptAsyncWithResultMethodInfo = 
            typeof(BaseInterceptor<TState>).GetMethod("InterceptAsyncWithResult", BindingFlags.Instance | BindingFlags.NonPublic);

        public void Intercept(IInvocation invocation)
        {
            var state = BeforeAction(invocation);

            try
            {
                invocation.Proceed();

                var delegateType = GetDelegateType(invocation);
                switch (delegateType)
                {
                    case MethodType.Synchronous:
                        OnSuccess(invocation, state);
                        break;
                    case MethodType.AsyncAction:
                        InterceptAsync(invocation, state);
                        break;
                    case MethodType.AsyncFunction:
                        ExecuteHandleAsyncWithResultUsingReflection(invocation, state);
                        break;
                }
            }
            catch (Exception ex)
            {
                OnFailure(invocation, ex, state);
                throw;
            }
        }

        public abstract TState BeforeAction(IInvocation invocation);
        public abstract void AfterAction(IInvocation invocation, TState state);
        public abstract void OnSuccessAction(IInvocation invocation, TState state);
        public abstract void OnFailureAction(IInvocation invocation, Exception exception, TState state);

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

        private void OnFailure(IInvocation invocation, Exception ex, TState state)
        {
            OnFailureAction(invocation, ex, state);
        }

        private void ExecuteHandleAsyncWithResultUsingReflection(IInvocation invocation, TState state)
        {
            var resultType = invocation.Method.ReturnType.GetGenericArguments()[0];
            var mi = interceptAsyncWithResultMethodInfo.MakeGenericMethod(resultType);
            invocation.ReturnValue = mi.Invoke(this, new object[] { invocation, state });
        }

        private async Task InterceptAsync(IInvocation invocation, TState state)
        {
            var task = invocation.ReturnValue as Task;
            try
            {
                await task;
                invocation.ReturnValue = null;
                OnSuccess(invocation, state);
            }
            catch (Exception ex)
            {
                OnFailure(invocation, ex, state);
                throw;
            }
        }

        private static MethodType GetDelegateType(IInvocation invocation)
        {
            var returnType = invocation.Method.ReturnType;
            if (returnType == typeof(Task))
                return MethodType.AsyncAction;
            if (returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(Task<>))
                return MethodType.AsyncFunction;
            return MethodType.Synchronous;
        }

        private enum MethodType
        {
            Synchronous,
            AsyncAction,
            AsyncFunction
        }
    }
}
