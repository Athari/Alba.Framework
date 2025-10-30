using Avalonia.Threading;

namespace Alba.Framework.Avalonia;

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
}