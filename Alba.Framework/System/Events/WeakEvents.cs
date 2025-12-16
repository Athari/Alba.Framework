using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Input;

namespace Alba.Framework;

public class WeakEvents
{
    public static readonly WeakEvent<INotifyCollectionChanged, NotifyCollectionChangedEventArgs> CollectionChanged =
        WeakEvent.Register<INotifyCollectionChanged, NotifyCollectionChangedEventArgs>((o, h) => {
            NotifyCollectionChangedEventHandler handler = (_, e) => h(o, e);
            o.CollectionChanged += handler;
            return () => o.CollectionChanged -= handler;
        });

    public static readonly WeakEvent<INotifyPropertyChanged, PropertyChangedEventArgs> PropertyChanged =
        WeakEvent.Register<INotifyPropertyChanged, PropertyChangedEventArgs>((o, h) => {
            PropertyChangedEventHandler handler = (_, e) => h(o, e);
            o.PropertyChanged += handler;
            return () => o.PropertyChanged -= handler;
        });


    public static readonly WeakEvent<ICommand, EventArgs> CommandCanExecuteChanged =
        WeakEvent.Register<ICommand>(
            (o, h) => o.CanExecuteChanged += h,
            (o, h) => o.CanExecuteChanged -= h);
}