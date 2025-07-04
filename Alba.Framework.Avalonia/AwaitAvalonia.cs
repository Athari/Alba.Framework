using System.Runtime.CompilerServices;
using Avalonia.Threading;

namespace Alba.Framework.Avalonia;

// ReSharper disable MethodOverloadWithOptionalParameter (GetAwaiter method must have no parameters to be recognized)
public static partial class AwaitAvalonia
{
    public static AvaloniaDispatcherAwaiter GetAwaiter(this DispatcherPriority @this) =>
        new(@this);

    public static AvaloniaDispatcherAwaiter GetAwaiter(this DispatcherPriority @this, bool alwaysYield = false) =>
        new(@this, alwaysYield);

    [PublicAPI]
    public readonly struct AvaloniaDispatcherAwaiter(DispatcherPriority priority, bool alwaysYield = false) : INotifyCompletion
    {
        public bool IsCompleted => !alwaysYield && Dispatcher.UIThread.CheckAccess();
        public void OnCompleted(Action continuation) => Dispatcher.UIThread.InvokeAsync(continuation, priority);
        public AvaloniaDispatcherAwaiter GetAwaiter() => this;
        public void GetResult() { }
    }
}