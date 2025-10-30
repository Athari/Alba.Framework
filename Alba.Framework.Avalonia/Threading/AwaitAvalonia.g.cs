using Avalonia.Threading;

namespace Alba.Framework.Avalonia.Threading;

public static partial class AwaitAvalonia
{
    public static AvaloniaDispatcherAwaiter Send                 { get; } = new(DispatcherPriority.Send           , alwaysYield: false);
    public static AvaloniaDispatcherAwaiter SendYield            { get; } = new(DispatcherPriority.Send           , alwaysYield: true);
    public static AvaloniaDispatcherAwaiter Render               { get; } = new(DispatcherPriority.Render         , alwaysYield: false);
    public static AvaloniaDispatcherAwaiter RenderYield          { get; } = new(DispatcherPriority.Render         , alwaysYield: true);
    public static AvaloniaDispatcherAwaiter Loaded               { get; } = new(DispatcherPriority.Loaded         , alwaysYield: false);
    public static AvaloniaDispatcherAwaiter LoadedYield          { get; } = new(DispatcherPriority.Loaded         , alwaysYield: true);
    public static AvaloniaDispatcherAwaiter Default              { get; } = new(DispatcherPriority.Default        , alwaysYield: false);
    public static AvaloniaDispatcherAwaiter DefaultYield         { get; } = new(DispatcherPriority.Default        , alwaysYield: true);
    public static AvaloniaDispatcherAwaiter Input                { get; } = new(DispatcherPriority.Input          , alwaysYield: false);
    public static AvaloniaDispatcherAwaiter InputYield           { get; } = new(DispatcherPriority.Input          , alwaysYield: true);
    public static AvaloniaDispatcherAwaiter Background           { get; } = new(DispatcherPriority.Background     , alwaysYield: false);
    public static AvaloniaDispatcherAwaiter BackgroundYield      { get; } = new(DispatcherPriority.Background     , alwaysYield: true);
    public static AvaloniaDispatcherAwaiter ContextIdle          { get; } = new(DispatcherPriority.ContextIdle    , alwaysYield: false);
    public static AvaloniaDispatcherAwaiter ContextIdleYield     { get; } = new(DispatcherPriority.ContextIdle    , alwaysYield: true);
    public static AvaloniaDispatcherAwaiter ApplicationIdle      { get; } = new(DispatcherPriority.ApplicationIdle, alwaysYield: false);
    public static AvaloniaDispatcherAwaiter ApplicationIdleYield { get; } = new(DispatcherPriority.ApplicationIdle, alwaysYield: true);
    public static AvaloniaDispatcherAwaiter SystemIdle           { get; } = new(DispatcherPriority.SystemIdle     , alwaysYield: false);
    public static AvaloniaDispatcherAwaiter SystemIdleYield      { get; } = new(DispatcherPriority.SystemIdle     , alwaysYield: true);

    public static void InvokeOnSend(Action callback)
        => Dispatcher.UIThread.Invoke(callback, DispatcherPriority.Send);

    public static void InvokeOnSend(CancellationToken ct, Action callback)
        => Dispatcher.UIThread.Invoke(callback, DispatcherPriority.Send, ct);

    public static void InvokeOnSend(TimeSpan timeout, Action callback)
        => Dispatcher.UIThread.Invoke(callback, DispatcherPriority.Send, CancellationToken.None, timeout);

    public static void InvokeOnSend(CancellationToken ct, TimeSpan timeout, Action callback)
        => Dispatcher.UIThread.Invoke(callback, DispatcherPriority.Send, ct, timeout);


    public static void InvokeOnSend<T>(Func<T> callback)
        => Dispatcher.UIThread.Invoke(callback, DispatcherPriority.Send);

    public static void InvokeOnSend<T>(CancellationToken ct, Func<T> callback)
        => Dispatcher.UIThread.Invoke(callback, DispatcherPriority.Send, ct);

    public static void InvokeOnSend<T>(TimeSpan timeout, Func<T> callback)
        => Dispatcher.UIThread.Invoke(callback, DispatcherPriority.Send, CancellationToken.None, timeout);

    public static void InvokeOnSend<T>(CancellationToken ct, TimeSpan timeout, Func<T> callback)
        => Dispatcher.UIThread.Invoke(callback, DispatcherPriority.Send, ct, timeout);


    public static DispatcherOperation InvokeOnSendAsync(Action callback)
        => Dispatcher.UIThread.InvokeAsync(callback, DispatcherPriority.Send);

    public static DispatcherOperation InvokeOnSendAsync(CancellationToken ct, Action callback)
        => Dispatcher.UIThread.InvokeAsync(callback, DispatcherPriority.Send, ct);


    public static DispatcherOperation<T> InvokeOnSendAsync<T>(Func<T> callback)
        => Dispatcher.UIThread.InvokeAsync(callback, DispatcherPriority.Send);

    public static DispatcherOperation<T> InvokeOnSendAsync<T>(CancellationToken ct, Func<T> callback)
        => Dispatcher.UIThread.InvokeAsync(callback, DispatcherPriority.Send, ct);


    public static Task InvokeOnSendAsync(Func<Task> callback)
        => Dispatcher.UIThread.InvokeAsync(callback, DispatcherPriority.Send);

    public static Task<T> InvokeOnSendAsync<T>(Func<Task<T>> callback)
        => Dispatcher.UIThread.InvokeAsync(callback, DispatcherPriority.Send);


    public static void InvokeOnRender(Action callback)
        => Dispatcher.UIThread.Invoke(callback, DispatcherPriority.Render);

    public static void InvokeOnRender(CancellationToken ct, Action callback)
        => Dispatcher.UIThread.Invoke(callback, DispatcherPriority.Render, ct);

    public static void InvokeOnRender(TimeSpan timeout, Action callback)
        => Dispatcher.UIThread.Invoke(callback, DispatcherPriority.Render, CancellationToken.None, timeout);

    public static void InvokeOnRender(CancellationToken ct, TimeSpan timeout, Action callback)
        => Dispatcher.UIThread.Invoke(callback, DispatcherPriority.Render, ct, timeout);


    public static void InvokeOnRender<T>(Func<T> callback)
        => Dispatcher.UIThread.Invoke(callback, DispatcherPriority.Render);

    public static void InvokeOnRender<T>(CancellationToken ct, Func<T> callback)
        => Dispatcher.UIThread.Invoke(callback, DispatcherPriority.Render, ct);

    public static void InvokeOnRender<T>(TimeSpan timeout, Func<T> callback)
        => Dispatcher.UIThread.Invoke(callback, DispatcherPriority.Render, CancellationToken.None, timeout);

    public static void InvokeOnRender<T>(CancellationToken ct, TimeSpan timeout, Func<T> callback)
        => Dispatcher.UIThread.Invoke(callback, DispatcherPriority.Render, ct, timeout);


    public static DispatcherOperation InvokeOnRenderAsync(Action callback)
        => Dispatcher.UIThread.InvokeAsync(callback, DispatcherPriority.Render);

    public static DispatcherOperation InvokeOnRenderAsync(CancellationToken ct, Action callback)
        => Dispatcher.UIThread.InvokeAsync(callback, DispatcherPriority.Render, ct);


    public static DispatcherOperation<T> InvokeOnRenderAsync<T>(Func<T> callback)
        => Dispatcher.UIThread.InvokeAsync(callback, DispatcherPriority.Render);

    public static DispatcherOperation<T> InvokeOnRenderAsync<T>(CancellationToken ct, Func<T> callback)
        => Dispatcher.UIThread.InvokeAsync(callback, DispatcherPriority.Render, ct);


    public static Task InvokeOnRenderAsync(Func<Task> callback)
        => Dispatcher.UIThread.InvokeAsync(callback, DispatcherPriority.Render);

    public static Task<T> InvokeOnRenderAsync<T>(Func<Task<T>> callback)
        => Dispatcher.UIThread.InvokeAsync(callback, DispatcherPriority.Render);


    public static void InvokeOnLoaded(Action callback)
        => Dispatcher.UIThread.Invoke(callback, DispatcherPriority.Loaded);

    public static void InvokeOnLoaded(CancellationToken ct, Action callback)
        => Dispatcher.UIThread.Invoke(callback, DispatcherPriority.Loaded, ct);

    public static void InvokeOnLoaded(TimeSpan timeout, Action callback)
        => Dispatcher.UIThread.Invoke(callback, DispatcherPriority.Loaded, CancellationToken.None, timeout);

    public static void InvokeOnLoaded(CancellationToken ct, TimeSpan timeout, Action callback)
        => Dispatcher.UIThread.Invoke(callback, DispatcherPriority.Loaded, ct, timeout);


    public static void InvokeOnLoaded<T>(Func<T> callback)
        => Dispatcher.UIThread.Invoke(callback, DispatcherPriority.Loaded);

    public static void InvokeOnLoaded<T>(CancellationToken ct, Func<T> callback)
        => Dispatcher.UIThread.Invoke(callback, DispatcherPriority.Loaded, ct);

    public static void InvokeOnLoaded<T>(TimeSpan timeout, Func<T> callback)
        => Dispatcher.UIThread.Invoke(callback, DispatcherPriority.Loaded, CancellationToken.None, timeout);

    public static void InvokeOnLoaded<T>(CancellationToken ct, TimeSpan timeout, Func<T> callback)
        => Dispatcher.UIThread.Invoke(callback, DispatcherPriority.Loaded, ct, timeout);


    public static DispatcherOperation InvokeOnLoadedAsync(Action callback)
        => Dispatcher.UIThread.InvokeAsync(callback, DispatcherPriority.Loaded);

    public static DispatcherOperation InvokeOnLoadedAsync(CancellationToken ct, Action callback)
        => Dispatcher.UIThread.InvokeAsync(callback, DispatcherPriority.Loaded, ct);


    public static DispatcherOperation<T> InvokeOnLoadedAsync<T>(Func<T> callback)
        => Dispatcher.UIThread.InvokeAsync(callback, DispatcherPriority.Loaded);

    public static DispatcherOperation<T> InvokeOnLoadedAsync<T>(CancellationToken ct, Func<T> callback)
        => Dispatcher.UIThread.InvokeAsync(callback, DispatcherPriority.Loaded, ct);


    public static Task InvokeOnLoadedAsync(Func<Task> callback)
        => Dispatcher.UIThread.InvokeAsync(callback, DispatcherPriority.Loaded);

    public static Task<T> InvokeOnLoadedAsync<T>(Func<Task<T>> callback)
        => Dispatcher.UIThread.InvokeAsync(callback, DispatcherPriority.Loaded);


    public static void InvokeOnDefault(Action callback)
        => Dispatcher.UIThread.Invoke(callback, DispatcherPriority.Default);

    public static void InvokeOnDefault(CancellationToken ct, Action callback)
        => Dispatcher.UIThread.Invoke(callback, DispatcherPriority.Default, ct);

    public static void InvokeOnDefault(TimeSpan timeout, Action callback)
        => Dispatcher.UIThread.Invoke(callback, DispatcherPriority.Default, CancellationToken.None, timeout);

    public static void InvokeOnDefault(CancellationToken ct, TimeSpan timeout, Action callback)
        => Dispatcher.UIThread.Invoke(callback, DispatcherPriority.Default, ct, timeout);


    public static void InvokeOnDefault<T>(Func<T> callback)
        => Dispatcher.UIThread.Invoke(callback, DispatcherPriority.Default);

    public static void InvokeOnDefault<T>(CancellationToken ct, Func<T> callback)
        => Dispatcher.UIThread.Invoke(callback, DispatcherPriority.Default, ct);

    public static void InvokeOnDefault<T>(TimeSpan timeout, Func<T> callback)
        => Dispatcher.UIThread.Invoke(callback, DispatcherPriority.Default, CancellationToken.None, timeout);

    public static void InvokeOnDefault<T>(CancellationToken ct, TimeSpan timeout, Func<T> callback)
        => Dispatcher.UIThread.Invoke(callback, DispatcherPriority.Default, ct, timeout);


    public static DispatcherOperation InvokeOnDefaultAsync(Action callback)
        => Dispatcher.UIThread.InvokeAsync(callback, DispatcherPriority.Default);

    public static DispatcherOperation InvokeOnDefaultAsync(CancellationToken ct, Action callback)
        => Dispatcher.UIThread.InvokeAsync(callback, DispatcherPriority.Default, ct);


    public static DispatcherOperation<T> InvokeOnDefaultAsync<T>(Func<T> callback)
        => Dispatcher.UIThread.InvokeAsync(callback, DispatcherPriority.Default);

    public static DispatcherOperation<T> InvokeOnDefaultAsync<T>(CancellationToken ct, Func<T> callback)
        => Dispatcher.UIThread.InvokeAsync(callback, DispatcherPriority.Default, ct);


    public static Task InvokeOnDefaultAsync(Func<Task> callback)
        => Dispatcher.UIThread.InvokeAsync(callback, DispatcherPriority.Default);

    public static Task<T> InvokeOnDefaultAsync<T>(Func<Task<T>> callback)
        => Dispatcher.UIThread.InvokeAsync(callback, DispatcherPriority.Default);


    public static void InvokeOnInput(Action callback)
        => Dispatcher.UIThread.Invoke(callback, DispatcherPriority.Input);

    public static void InvokeOnInput(CancellationToken ct, Action callback)
        => Dispatcher.UIThread.Invoke(callback, DispatcherPriority.Input, ct);

    public static void InvokeOnInput(TimeSpan timeout, Action callback)
        => Dispatcher.UIThread.Invoke(callback, DispatcherPriority.Input, CancellationToken.None, timeout);

    public static void InvokeOnInput(CancellationToken ct, TimeSpan timeout, Action callback)
        => Dispatcher.UIThread.Invoke(callback, DispatcherPriority.Input, ct, timeout);


    public static void InvokeOnInput<T>(Func<T> callback)
        => Dispatcher.UIThread.Invoke(callback, DispatcherPriority.Input);

    public static void InvokeOnInput<T>(CancellationToken ct, Func<T> callback)
        => Dispatcher.UIThread.Invoke(callback, DispatcherPriority.Input, ct);

    public static void InvokeOnInput<T>(TimeSpan timeout, Func<T> callback)
        => Dispatcher.UIThread.Invoke(callback, DispatcherPriority.Input, CancellationToken.None, timeout);

    public static void InvokeOnInput<T>(CancellationToken ct, TimeSpan timeout, Func<T> callback)
        => Dispatcher.UIThread.Invoke(callback, DispatcherPriority.Input, ct, timeout);


    public static DispatcherOperation InvokeOnInputAsync(Action callback)
        => Dispatcher.UIThread.InvokeAsync(callback, DispatcherPriority.Input);

    public static DispatcherOperation InvokeOnInputAsync(CancellationToken ct, Action callback)
        => Dispatcher.UIThread.InvokeAsync(callback, DispatcherPriority.Input, ct);


    public static DispatcherOperation<T> InvokeOnInputAsync<T>(Func<T> callback)
        => Dispatcher.UIThread.InvokeAsync(callback, DispatcherPriority.Input);

    public static DispatcherOperation<T> InvokeOnInputAsync<T>(CancellationToken ct, Func<T> callback)
        => Dispatcher.UIThread.InvokeAsync(callback, DispatcherPriority.Input, ct);


    public static Task InvokeOnInputAsync(Func<Task> callback)
        => Dispatcher.UIThread.InvokeAsync(callback, DispatcherPriority.Input);

    public static Task<T> InvokeOnInputAsync<T>(Func<Task<T>> callback)
        => Dispatcher.UIThread.InvokeAsync(callback, DispatcherPriority.Input);


    public static void InvokeOnBackground(Action callback)
        => Dispatcher.UIThread.Invoke(callback, DispatcherPriority.Background);

    public static void InvokeOnBackground(CancellationToken ct, Action callback)
        => Dispatcher.UIThread.Invoke(callback, DispatcherPriority.Background, ct);

    public static void InvokeOnBackground(TimeSpan timeout, Action callback)
        => Dispatcher.UIThread.Invoke(callback, DispatcherPriority.Background, CancellationToken.None, timeout);

    public static void InvokeOnBackground(CancellationToken ct, TimeSpan timeout, Action callback)
        => Dispatcher.UIThread.Invoke(callback, DispatcherPriority.Background, ct, timeout);


    public static void InvokeOnBackground<T>(Func<T> callback)
        => Dispatcher.UIThread.Invoke(callback, DispatcherPriority.Background);

    public static void InvokeOnBackground<T>(CancellationToken ct, Func<T> callback)
        => Dispatcher.UIThread.Invoke(callback, DispatcherPriority.Background, ct);

    public static void InvokeOnBackground<T>(TimeSpan timeout, Func<T> callback)
        => Dispatcher.UIThread.Invoke(callback, DispatcherPriority.Background, CancellationToken.None, timeout);

    public static void InvokeOnBackground<T>(CancellationToken ct, TimeSpan timeout, Func<T> callback)
        => Dispatcher.UIThread.Invoke(callback, DispatcherPriority.Background, ct, timeout);


    public static DispatcherOperation InvokeOnBackgroundAsync(Action callback)
        => Dispatcher.UIThread.InvokeAsync(callback, DispatcherPriority.Background);

    public static DispatcherOperation InvokeOnBackgroundAsync(CancellationToken ct, Action callback)
        => Dispatcher.UIThread.InvokeAsync(callback, DispatcherPriority.Background, ct);


    public static DispatcherOperation<T> InvokeOnBackgroundAsync<T>(Func<T> callback)
        => Dispatcher.UIThread.InvokeAsync(callback, DispatcherPriority.Background);

    public static DispatcherOperation<T> InvokeOnBackgroundAsync<T>(CancellationToken ct, Func<T> callback)
        => Dispatcher.UIThread.InvokeAsync(callback, DispatcherPriority.Background, ct);


    public static Task InvokeOnBackgroundAsync(Func<Task> callback)
        => Dispatcher.UIThread.InvokeAsync(callback, DispatcherPriority.Background);

    public static Task<T> InvokeOnBackgroundAsync<T>(Func<Task<T>> callback)
        => Dispatcher.UIThread.InvokeAsync(callback, DispatcherPriority.Background);


    public static void InvokeOnContextIdle(Action callback)
        => Dispatcher.UIThread.Invoke(callback, DispatcherPriority.ContextIdle);

    public static void InvokeOnContextIdle(CancellationToken ct, Action callback)
        => Dispatcher.UIThread.Invoke(callback, DispatcherPriority.ContextIdle, ct);

    public static void InvokeOnContextIdle(TimeSpan timeout, Action callback)
        => Dispatcher.UIThread.Invoke(callback, DispatcherPriority.ContextIdle, CancellationToken.None, timeout);

    public static void InvokeOnContextIdle(CancellationToken ct, TimeSpan timeout, Action callback)
        => Dispatcher.UIThread.Invoke(callback, DispatcherPriority.ContextIdle, ct, timeout);


    public static void InvokeOnContextIdle<T>(Func<T> callback)
        => Dispatcher.UIThread.Invoke(callback, DispatcherPriority.ContextIdle);

    public static void InvokeOnContextIdle<T>(CancellationToken ct, Func<T> callback)
        => Dispatcher.UIThread.Invoke(callback, DispatcherPriority.ContextIdle, ct);

    public static void InvokeOnContextIdle<T>(TimeSpan timeout, Func<T> callback)
        => Dispatcher.UIThread.Invoke(callback, DispatcherPriority.ContextIdle, CancellationToken.None, timeout);

    public static void InvokeOnContextIdle<T>(CancellationToken ct, TimeSpan timeout, Func<T> callback)
        => Dispatcher.UIThread.Invoke(callback, DispatcherPriority.ContextIdle, ct, timeout);


    public static DispatcherOperation InvokeOnContextIdleAsync(Action callback)
        => Dispatcher.UIThread.InvokeAsync(callback, DispatcherPriority.ContextIdle);

    public static DispatcherOperation InvokeOnContextIdleAsync(CancellationToken ct, Action callback)
        => Dispatcher.UIThread.InvokeAsync(callback, DispatcherPriority.ContextIdle, ct);


    public static DispatcherOperation<T> InvokeOnContextIdleAsync<T>(Func<T> callback)
        => Dispatcher.UIThread.InvokeAsync(callback, DispatcherPriority.ContextIdle);

    public static DispatcherOperation<T> InvokeOnContextIdleAsync<T>(CancellationToken ct, Func<T> callback)
        => Dispatcher.UIThread.InvokeAsync(callback, DispatcherPriority.ContextIdle, ct);


    public static Task InvokeOnContextIdleAsync(Func<Task> callback)
        => Dispatcher.UIThread.InvokeAsync(callback, DispatcherPriority.ContextIdle);

    public static Task<T> InvokeOnContextIdleAsync<T>(Func<Task<T>> callback)
        => Dispatcher.UIThread.InvokeAsync(callback, DispatcherPriority.ContextIdle);


    public static void InvokeOnApplicationIdle(Action callback)
        => Dispatcher.UIThread.Invoke(callback, DispatcherPriority.ApplicationIdle);

    public static void InvokeOnApplicationIdle(CancellationToken ct, Action callback)
        => Dispatcher.UIThread.Invoke(callback, DispatcherPriority.ApplicationIdle, ct);

    public static void InvokeOnApplicationIdle(TimeSpan timeout, Action callback)
        => Dispatcher.UIThread.Invoke(callback, DispatcherPriority.ApplicationIdle, CancellationToken.None, timeout);

    public static void InvokeOnApplicationIdle(CancellationToken ct, TimeSpan timeout, Action callback)
        => Dispatcher.UIThread.Invoke(callback, DispatcherPriority.ApplicationIdle, ct, timeout);


    public static void InvokeOnApplicationIdle<T>(Func<T> callback)
        => Dispatcher.UIThread.Invoke(callback, DispatcherPriority.ApplicationIdle);

    public static void InvokeOnApplicationIdle<T>(CancellationToken ct, Func<T> callback)
        => Dispatcher.UIThread.Invoke(callback, DispatcherPriority.ApplicationIdle, ct);

    public static void InvokeOnApplicationIdle<T>(TimeSpan timeout, Func<T> callback)
        => Dispatcher.UIThread.Invoke(callback, DispatcherPriority.ApplicationIdle, CancellationToken.None, timeout);

    public static void InvokeOnApplicationIdle<T>(CancellationToken ct, TimeSpan timeout, Func<T> callback)
        => Dispatcher.UIThread.Invoke(callback, DispatcherPriority.ApplicationIdle, ct, timeout);


    public static DispatcherOperation InvokeOnApplicationIdleAsync(Action callback)
        => Dispatcher.UIThread.InvokeAsync(callback, DispatcherPriority.ApplicationIdle);

    public static DispatcherOperation InvokeOnApplicationIdleAsync(CancellationToken ct, Action callback)
        => Dispatcher.UIThread.InvokeAsync(callback, DispatcherPriority.ApplicationIdle, ct);


    public static DispatcherOperation<T> InvokeOnApplicationIdleAsync<T>(Func<T> callback)
        => Dispatcher.UIThread.InvokeAsync(callback, DispatcherPriority.ApplicationIdle);

    public static DispatcherOperation<T> InvokeOnApplicationIdleAsync<T>(CancellationToken ct, Func<T> callback)
        => Dispatcher.UIThread.InvokeAsync(callback, DispatcherPriority.ApplicationIdle, ct);


    public static Task InvokeOnApplicationIdleAsync(Func<Task> callback)
        => Dispatcher.UIThread.InvokeAsync(callback, DispatcherPriority.ApplicationIdle);

    public static Task<T> InvokeOnApplicationIdleAsync<T>(Func<Task<T>> callback)
        => Dispatcher.UIThread.InvokeAsync(callback, DispatcherPriority.ApplicationIdle);


    public static void InvokeOnSystemIdle(Action callback)
        => Dispatcher.UIThread.Invoke(callback, DispatcherPriority.SystemIdle);

    public static void InvokeOnSystemIdle(CancellationToken ct, Action callback)
        => Dispatcher.UIThread.Invoke(callback, DispatcherPriority.SystemIdle, ct);

    public static void InvokeOnSystemIdle(TimeSpan timeout, Action callback)
        => Dispatcher.UIThread.Invoke(callback, DispatcherPriority.SystemIdle, CancellationToken.None, timeout);

    public static void InvokeOnSystemIdle(CancellationToken ct, TimeSpan timeout, Action callback)
        => Dispatcher.UIThread.Invoke(callback, DispatcherPriority.SystemIdle, ct, timeout);


    public static void InvokeOnSystemIdle<T>(Func<T> callback)
        => Dispatcher.UIThread.Invoke(callback, DispatcherPriority.SystemIdle);

    public static void InvokeOnSystemIdle<T>(CancellationToken ct, Func<T> callback)
        => Dispatcher.UIThread.Invoke(callback, DispatcherPriority.SystemIdle, ct);

    public static void InvokeOnSystemIdle<T>(TimeSpan timeout, Func<T> callback)
        => Dispatcher.UIThread.Invoke(callback, DispatcherPriority.SystemIdle, CancellationToken.None, timeout);

    public static void InvokeOnSystemIdle<T>(CancellationToken ct, TimeSpan timeout, Func<T> callback)
        => Dispatcher.UIThread.Invoke(callback, DispatcherPriority.SystemIdle, ct, timeout);


    public static DispatcherOperation InvokeOnSystemIdleAsync(Action callback)
        => Dispatcher.UIThread.InvokeAsync(callback, DispatcherPriority.SystemIdle);

    public static DispatcherOperation InvokeOnSystemIdleAsync(CancellationToken ct, Action callback)
        => Dispatcher.UIThread.InvokeAsync(callback, DispatcherPriority.SystemIdle, ct);


    public static DispatcherOperation<T> InvokeOnSystemIdleAsync<T>(Func<T> callback)
        => Dispatcher.UIThread.InvokeAsync(callback, DispatcherPriority.SystemIdle);

    public static DispatcherOperation<T> InvokeOnSystemIdleAsync<T>(CancellationToken ct, Func<T> callback)
        => Dispatcher.UIThread.InvokeAsync(callback, DispatcherPriority.SystemIdle, ct);


    public static Task InvokeOnSystemIdleAsync(Func<Task> callback)
        => Dispatcher.UIThread.InvokeAsync(callback, DispatcherPriority.SystemIdle);

    public static Task<T> InvokeOnSystemIdleAsync<T>(Func<Task<T>> callback)
        => Dispatcher.UIThread.InvokeAsync(callback, DispatcherPriority.SystemIdle);

}