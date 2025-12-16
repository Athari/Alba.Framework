namespace Alba.Framework;

/// <remarks>From https://github.com/AvaloniaUI/Avalonia/blob/master/src/Avalonia.Base/Utilities/WeakEvent.cs</remarks>
public class WeakEvent
{
    public static Action<Action> ScheduleCompact { get; set; } = a => ThreadPool.QueueUserWorkItem(_ => a());

    public static WeakEvent<TSender, TEventArgs> Register<TSender, TEventArgs>(
        Action<TSender, EventHandler<TEventArgs>> subscribe,
        Action<TSender, EventHandler<TEventArgs>> unsubscribe)
        where TSender : class =>
        new(subscribe, unsubscribe, ScheduleCompact);

    public static WeakEvent<TSender, TEventArgs> Register<TSender, TEventArgs>(
        Func<TSender, EventHandler<TEventArgs>, Action> subscribe)
        where TSender : class
        where TEventArgs : EventArgs =>
        new(subscribe, ScheduleCompact);

    public static WeakEvent<TSender, EventArgs> Register<TSender>(
        Action<TSender, EventHandler> subscribe,
        Action<TSender, EventHandler> unsubscribe)
        where TSender : class =>
        Register<TSender, EventArgs>((s, h) => {
            EventHandler handler = (_, e) => h(s, e);
            subscribe(s, handler);
            return () => unsubscribe(s, handler);
        });
}