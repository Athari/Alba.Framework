using System.Runtime.CompilerServices;
using Collections.Pooled;

namespace Alba.Framework;

/// <remarks>From https://github.com/AvaloniaUI/Avalonia/blob/master/src/Avalonia.Base/Utilities/WeakEvent.cs</remarks>
public sealed class WeakEvent<TSender, TEventArgs> : WeakEvent
    where TSender : class
{
    private readonly Func<TSender, EventHandler<TEventArgs>, Action> _subscribe;
    private readonly ConditionalWeakTable<TSender, Subscription> _subscriptions = new();
    private readonly ConditionalWeakTable<TSender, Subscription>.CreateValueCallback _createSubscription;
    private readonly Action<Action> _schedule;

    internal WeakEvent(
        Action<TSender, EventHandler<TEventArgs>> subscribe,
        Action<TSender, EventHandler<TEventArgs>> unsubscribe,
        Action<Action> schedule)
    {
        _subscribe = (o, h) => {
            subscribe(o, h);
            return () => unsubscribe(o, h);
        };
        _createSubscription = CreateSubscription;
        _schedule = schedule;
    }

    internal WeakEvent(
        Func<TSender, EventHandler<TEventArgs>, Action> subscribe,
        Action<Action> schedule)
    {
        _subscribe = subscribe;
        _createSubscription = CreateSubscription;
        _schedule = schedule;
    }

    public void Subscribe(TSender target, IWeakEventSubscriber<TEventArgs> subscriber)
    {
        // If subscription removed in another thread, adding subscriber can fail; retry until success
        var spinWait = default(SpinWait);
        while (true) {
            var subscription = _subscriptions.GetValue(target, _createSubscription);
            if (subscription.Add(subscriber))
                break;
            spinWait.SpinOnce();
        }
    }

    public void Unsubscribe(TSender target, IWeakEventSubscriber<TEventArgs> subscriber)
    {
        if (_subscriptions.TryGetValue(target, out var subscription))
            subscription.Remove(subscriber);
    }

    private Subscription CreateSubscription(TSender key) => new(this, key);

    private sealed class Subscription(WeakEvent<TSender, TEventArgs> ev, TSender target)
    {
        private readonly WeakHashList<IWeakEventSubscriber<TEventArgs>> _list = new();
        private readonly Lock _lock = new();
        private Action? _unsubscribe;
        private bool _compactScheduled;
        private bool _destroyed;

        private void Destroy()
        {
            if (_destroyed)
                return;
            _destroyed = true;
            _unsubscribe?.Invoke();
            ev._subscriptions.Remove(target);
        }

        public bool Add(IWeakEventSubscriber<TEventArgs> s)
        {
            if (_destroyed)
                return false;

            lock (_lock) {
                if (_destroyed)
                    return false;

                _unsubscribe ??= ev._subscribe(target, OnEvent);
                _list.Add(s);
                return true;
            }
        }

        public void Remove(IWeakEventSubscriber<TEventArgs> s)
        {
            if (_destroyed)
                return;

            lock (_lock) {
                if (_destroyed)
                    return;

                _list.Remove(s);
                if (_list.IsEmpty)
                    Destroy();
                else if (_list.NeedCompact && _compactScheduled)
                    ScheduleCompact();
            }
        }

        private void ScheduleCompact()
        {
            if (_compactScheduled || _destroyed)
                return;
            _compactScheduled = true;
            ev._schedule(Compact);
        }

        private void Compact()
        {
            if (_destroyed)
                return;

            lock (_lock) {
                if (_destroyed)
                    return;
                if (!_compactScheduled)
                    return;
                _compactScheduled = false;
                _list.Compact();
                if (_list.IsEmpty)
                    Destroy();
            }
        }

        private void OnEvent(object? sender, TEventArgs eventArgs)
        {
            PooledList<IWeakEventSubscriber<TEventArgs>>? alive;
            lock (_lock) {
                alive = _list.GetAlive();
                if (alive == null) {
                    Destroy();
                    return;
                }
            }

            foreach (var item in alive.Span)
                item.OnEvent(target, ev, eventArgs);

            lock (_lock) {
                WeakHashList<IWeakEventSubscriber<TEventArgs>>.ReturnToSharedPool(alive);
                if (_list.NeedCompact && !_compactScheduled)
                    ScheduleCompact();
            }
        }
    }
}