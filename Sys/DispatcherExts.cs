using System;
using System.Windows.Threading;

namespace Alba.Framework.Sys
{
    public static class DispatcherExts
    {
        private static readonly Action doNothing = () => { };

        public static void WaitForPriority (this Dispatcher @this, DispatcherPriority priority)
        {
            @this.Invoke(priority, doNothing);
        }

        public static void ExecuteAsync (this Dispatcher @this, Action action)
        {
            @this.BeginInvoke(action);
        }

        public static void ExecuteAsync (this Dispatcher @this, DispatcherPriority priority, Action action)
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
    }
}