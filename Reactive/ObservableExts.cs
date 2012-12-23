using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;

namespace Alba.Framework.Reactive
{
    public static class ObservableExts
    {
        public static IObservable<T> LimitMinDelay<T> (this IObservable<T> @this, TimeSpan delay)
        {
            var els = new EventLoopScheduler();
            return @this
                .ObserveOn(els)
                .Do(x => els.Schedule(() => Thread.Sleep(delay)));
        }

        public static IObservable<T> Flatten<T> (this IObservable<IObservable<T>> @this)
        {
            return @this.SelectMany(o => o);
        }
    }
}