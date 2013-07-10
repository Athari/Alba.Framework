using System;
using System.ComponentModel;
using System.Reactive;
using System.Reactive.Linq;

namespace Alba.Framework.Reactive
{
    public static class NotifyPropertyChangeExts
    {
        public static IObservable<EventPattern<T, PropertyChangingEventArgs>> ObservePropertyChanging<T> (this T @this)
            where T : INotifyPropertyChanging
        {
            return Observable.FromEventPattern<PropertyChangingEventHandler, PropertyChangingEventArgs>(
                h => @this.PropertyChanging += h, h => @this.PropertyChanging -= h)
                .Select(e => new EventPattern<T, PropertyChangingEventArgs>((T)e.Sender, e.EventArgs));
        }

        public static IObservable<EventPattern<T, PropertyChangedEventArgs>> ObservePropertyChanged<T> (this T @this)
            where T : INotifyPropertyChanged
        {
            return Observable.FromEventPattern<PropertyChangedEventHandler, PropertyChangedEventArgs>(
                h => @this.PropertyChanged += h, h => @this.PropertyChanged -= h)
                .Select(e => new EventPattern<T, PropertyChangedEventArgs>((T)e.Sender, e.EventArgs));
        }
    }
}