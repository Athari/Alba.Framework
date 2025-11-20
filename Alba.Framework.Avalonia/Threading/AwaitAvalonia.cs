using Alba.Framework.Threading;
using Avalonia.Threading;

namespace Alba.Framework.Avalonia.Threading;

[SuppressMessage("ReSharper", "MethodOverloadWithOptionalParameter", Justification = "GetAwaiter method must have no parameters to be recognized")]
[SuppressMessage("Design", "CA1068:CancellationToken parameters must come last", Justification = "Convenience methods which have callback last for readability")]
public static partial class AwaitAvalonia
{
    public static AvaloniaDispatcherAwaiter GetAwaiter(this DispatcherPriority @this) =>
        new(@this);

    public static AvaloniaDispatcherAwaiter GetAwaiter(this DispatcherPriority @this, bool alwaysYield = false) =>
        new(@this, alwaysYield);


    public static void Invoke(Action callback)
        => Dispatcher.UIThread.Invoke(callback);

    public static void Invoke(DispatcherPriority priority, Action callback)
        => Dispatcher.UIThread.Invoke(callback, priority);

    public static void Invoke(DispatcherPriority priority, CancellationToken ct, Action callback)
        => Dispatcher.UIThread.Invoke(callback, priority, ct);

    public static void Invoke(DispatcherPriority priority, TimeSpan timeout, Action callback)
        => Dispatcher.UIThread.Invoke(callback, priority, CancellationToken.None, timeout);

    public static void Invoke(DispatcherPriority priority, CancellationToken ct, TimeSpan timeout, Action callback)
        => Dispatcher.UIThread.Invoke(callback, priority, ct, timeout);


    public static void Invoke<T>(Func<T> callback)
        => Dispatcher.UIThread.Invoke(callback);

    public static void Invoke<T>(DispatcherPriority priority, Func<T> callback)
        => Dispatcher.UIThread.Invoke(callback, priority);

    public static void Invoke<T>(DispatcherPriority priority, CancellationToken ct, Func<T> callback)
        => Dispatcher.UIThread.Invoke(callback, priority, ct);

    public static void Invoke<T>(DispatcherPriority priority, TimeSpan timeout, Func<T> callback)
        => Dispatcher.UIThread.Invoke(callback, priority, CancellationToken.None, timeout);

    public static void Invoke<T>(DispatcherPriority priority, CancellationToken ct, TimeSpan timeout, Func<T> callback)
        => Dispatcher.UIThread.Invoke(callback, priority, ct, timeout);


    public static DispatcherOperation InvokeAsync(Action callback)
        => Dispatcher.UIThread.InvokeAsync(callback);

    public static DispatcherOperation InvokeAsync(DispatcherPriority priority, Action callback)
        => Dispatcher.UIThread.InvokeAsync(callback, priority);

    public static DispatcherOperation InvokeAsync(DispatcherPriority priority, CancellationToken ct, Action callback)
        => Dispatcher.UIThread.InvokeAsync(callback, priority, ct);


    public static DispatcherOperation<T> InvokeAsync<T>(Func<T> callback)
        => Dispatcher.UIThread.InvokeAsync(callback);

    public static DispatcherOperation<T> InvokeAsync<T>(DispatcherPriority priority, Func<T> callback)
        => Dispatcher.UIThread.InvokeAsync(callback, priority);

    public static DispatcherOperation<T> InvokeAsync<T>(DispatcherPriority priority, CancellationToken ct, Func<T> callback)
        => Dispatcher.UIThread.InvokeAsync(callback, priority, ct);


    public static Task InvokeAsync(Func<Task> callback)
        => Dispatcher.UIThread.InvokeAsync(callback);

    public static Task InvokeAsync(DispatcherPriority priority, Func<Task> callback)
        => Dispatcher.UIThread.InvokeAsync(callback, priority);


    public static Task<T> InvokeAsync<T>(Func<Task<T>> callback)
        => Dispatcher.UIThread.InvokeAsync(callback);

    public static Task<T> InvokeAsync<T>(DispatcherPriority priority, Func<Task<T>> callback)
        => Dispatcher.UIThread.InvokeAsync(callback, priority);


    public readonly struct AvaloniaDispatcherAwaiter(DispatcherPriority priority, bool alwaysYield = false)
        : IAwaiter<AvaloniaDispatcherAwaiter>
    {
        public bool IsCompleted => !alwaysYield && Dispatcher.UIThread.CheckAccess();
        public void OnCompleted(Action continuation) => Dispatcher.UIThread.InvokeAsync(continuation, priority);
        public void UnsafeOnCompleted(Action continuation) => OnCompleted(continuation);
        public AvaloniaDispatcherAwaiter GetAwaiter() => this;
        public void GetResult() { }
    }
}