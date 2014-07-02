using System;
using System.Threading.Tasks;
using System.Windows.Threading;
using Alba.Framework.Sys;

namespace Alba.Framework.Diagnostics
{
    public class UnhandledExceptionsHandler
    {
        public event EventHandler<UnhandledExceptionsEventArgs> UnhandledException;

        public UnhandledExceptionsHandler ()
        {
            Dispatcher.CurrentDispatcher.UnhandledException += CurrentDispatcher_DispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
        }

        private void CurrentDispatcher_DispatcherUnhandledException (object sender, DispatcherUnhandledExceptionEventArgs args)
        {
            var excArgs = new UnhandledExceptionsEventArgs(args.Exception, UnhandledExceptionSource.CurrentDispatcher);
            OnUnhandledException(excArgs);
            args.Handled = excArgs.Handled;
        }

        private void CurrentDomain_UnhandledException (object sender, UnhandledExceptionEventArgs args)
        {
            var excArgs = new UnhandledExceptionsEventArgs((Exception)args.ExceptionObject, UnhandledExceptionSource.CurrentDomain, false);
            OnUnhandledException(excArgs);
        }

        private void TaskScheduler_UnobservedTaskException (object sender, UnobservedTaskExceptionEventArgs args)
        {
            args.Exception.Flatten();
            var excArgs = new UnhandledExceptionsEventArgs(args.Exception.InnerExceptions, UnhandledExceptionSource.TaskScheduler);
            OnUnhandledException(excArgs);
            if (excArgs.Handled)
                args.SetObserved();
        }

        protected virtual void OnUnhandledException (UnhandledExceptionsEventArgs args)
        {
            UnhandledException.NullableInvoke(this, args);
        }
    }
}