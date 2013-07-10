using System;
using System.Reactive;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Alba.Framework.Sys
{
    public static class DispatcherExts
    {
        private static readonly Action doNothing = () => { };
        private static readonly Task EmptyTask = Task.FromResult(Unit.Default);

        public static void WaitForPriority (this Dispatcher @this, DispatcherPriority priority)
        {
            @this.Invoke(priority, doNothing);
        }

        public static void QueueExecute (this Dispatcher @this, Action action)
        {
            @this.BeginInvoke(action);
        }

        public static void QueueExecute (this Dispatcher @this, DispatcherPriority priority, Action action)
        {
            @this.BeginInvoke(priority, action);
        }

        public static void Execute (this Dispatcher @this, Action action)
        {
            if (@this.CheckAccess())
                action();
            else
                @this.Invoke(action);
        }

        public static T Execute<T> (this Dispatcher @this, Func<T> func)
        {
            return @this.CheckAccess() ? func() : @this.Invoke(func);
        }

        public static Task ExecuteAsync (this Dispatcher @this, Action action)
        {
            if (@this.CheckAccess()) {
                action();
                return EmptyTask;
            }
            else {
                return @this.InvokeAsync(action).Task;
            }
        }

        public static Task<T> ExecuteAsync<T> (this Dispatcher @this, Func<T> func)
        {
            if (@this.CheckAccess())
                return Task.FromResult(func());
            else
                return @this.InvokeAsync(func).Task;
        }
    }
}