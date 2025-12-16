using System.ComponentModel;
using Collections.Pooled;

namespace Alba.Framework;

public interface IWeakEventSubscriber<in TEventArgs>
{
    void OnEvent(object? sender, WeakEvent ev, TEventArgs e);
}

public sealed class WeakEventSubscriber<TEventArgs> : IWeakEventSubscriber<TEventArgs>
{
    public event Action<object?, WeakEvent, TEventArgs>? Event;

    void IWeakEventSubscriber<TEventArgs>.OnEvent(object? sender, WeakEvent ev, TEventArgs e) =>
        Event?.Invoke(sender, ev, e);
}

public sealed class WeakPropertyChangedEventSubscriber
    (PropertyChangedEventHandler? handler, params ReadOnlySpan<string> propertyNames)
    : IWeakEventSubscriber<PropertyChangedEventArgs>
{
    private readonly PooledList<string>? _propertyNames = propertyNames.Length > 0 ? [ ..propertyNames ] : null;

    void IWeakEventSubscriber<PropertyChangedEventArgs>.OnEvent(object? sender, WeakEvent ev, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == null || _propertyNames?.Contains(e.PropertyName) != false)
            handler?.Invoke(sender, e);
    }
}

public sealed class TargetWeakEventSubscriber<TTarget, TEventArgs>
    (TTarget target, Action<TTarget, object?, WeakEvent, TEventArgs> dispatchFunc)
    : IWeakEventSubscriber<TEventArgs>
{
    void IWeakEventSubscriber<TEventArgs>.OnEvent(object? sender, WeakEvent ev, TEventArgs e) =>
        dispatchFunc(target, sender, ev, e);
}