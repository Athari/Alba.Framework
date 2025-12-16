using System.ComponentModel;

namespace Alba.Framework;

public static class WeakEventExts
{
    extension<TSender>(WeakEvent<TSender, PropertyChangedEventArgs> @this)
        where TSender : class, INotifyPropertyChanged
    {
        public WeakPropertyChangedEventSubscriber Subscribe(TSender target, PropertyChangedEventHandler handler,
            params ReadOnlySpan<string> propertyNames)
        {
            var subscriber = new WeakPropertyChangedEventSubscriber(handler, propertyNames);
            @this.Subscribe(target, subscriber);
            return subscriber;
        }
    }

    extension<T>(T @this)
        where T : class, INotifyPropertyChanged
    {
        public IDisposable WeakSubscribePropertyChanged(PropertyChangedEventHandler handler,
            params ReadOnlySpan<string> propertyNames)
        {
            var sub = WeakEvents.PropertyChanged.Subscribe(@this, handler, propertyNames);
            return Create(WeakEvents.PropertyChanged, @this, sub);
        }
    }

    private class SubscriberDisposable<TSender, TEventArgs>
        (WeakEvent<TSender, TEventArgs> ev, TSender target, IWeakEventSubscriber<TEventArgs> subscriber)
        : IDisposable
        where TSender : class
    {
        public void Dispose()
        {
            ev.Unsubscribe(target, subscriber);
            GC.SuppressFinalize(this);
        }
    }

    private static SubscriberDisposable<TSender, TEventArgs> Create<TSender, TEventArgs>(
        WeakEvent<TSender, TEventArgs> ev, TSender target, IWeakEventSubscriber<TEventArgs> subscriber)
        where TSender : class =>
        new(ev, target, subscriber);
}