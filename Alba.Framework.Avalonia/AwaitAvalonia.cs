using System.Diagnostics.CodeAnalysis;
using Alba.Framework.Threading;
using Avalonia.Threading;

namespace Alba.Framework.Avalonia;

[PublicAPI]
[SuppressMessage("ReSharper", "MethodOverloadWithOptionalParameter", Justification = "GetAwaiter method must have no parameters to be recognized")]
public static partial class AwaitAvalonia
{
    public static AvaloniaDispatcherAwaiter GetAwaiter(this DispatcherPriority @this) =>
        new(@this);

    public static AvaloniaDispatcherAwaiter GetAwaiter(this DispatcherPriority @this, bool alwaysYield = false) =>
        new(@this, alwaysYield);

    [PublicAPI]
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