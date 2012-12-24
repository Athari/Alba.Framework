using System;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using Alba.Framework.Events;

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

        public static IObservable<EventPattern<EventArgs<TArgs>>> SelectEventPattern<T, TArgs> (this IObservable<T> @this,
            object sender, Func<T, TArgs> selector)
        {
            return @this.Select(o => new EventPattern<EventArgs<TArgs>>(sender, new EventArgs<TArgs>(selector(o))));
        }
    }
}