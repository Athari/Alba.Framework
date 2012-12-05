using System;
using System.Windows.Threading;

namespace Alba.Framework.System
{
    public static class UiDispatcher
    {
        private static readonly Action doNothing = () => { };

        private static Dispatcher AppDispatcher
        {
            get { return Dispatcher.CurrentDispatcher; }
        }

        public static void VerifyAccess ()
        {
            AppDispatcher.VerifyAccess();
        }

        public static bool CheckAccess ()
        {
            return AppDispatcher.CheckAccess();
        }

        public static void WaitForPriority (DispatcherPriority priority)
        {
            AppDispatcher.Invoke(priority, doNothing);
        }

        public static void ExecuteAsync (Action action)
        {
            AppDispatcher.BeginInvoke(action);
        }

        public static void Execute (Action action)
        {
            if (CheckAccess())
                action();
            else
                AppDispatcher.Invoke(action);
        }

        public static T Execute<T> (Func<T> func)
        {
            return CheckAccess() ? func() : AppDispatcher.Invoke(func);
        }

        /*public static IObservable<TSource> ObserveOnUiDispatcher<TSource> (this IObservable<TSource> source)
        {
            return Observable.CreateWithDisposable<TSource>(observer => source.Subscribe(
                x => {
                    if (CheckAccess())
                        observer.OnNext(x);
                    else
                        ExecuteAsync(observer.OnNext, x);
                },
                e => {
                    if (CheckAccess())
                        observer.OnError(e);
                    else
                        ExecuteAsync(observer.OnError, e);
                },
                () => {
                    if (CheckAccess())
                        observer.OnCompleted();
                    else
                        ExecuteAsync(observer.OnCompleted);
                }
                ));
        }*/
    }
}