#if OC_SUPPORT_AVALONIA

using System.Diagnostics.CodeAnalysis;
using Avalonia.Threading;
using ObservableComputations;

namespace Alba.Framework.Collections.ObservableComputations;

[PublicAPI]
public class AvaloniaOcDispatcher(Dispatcher dispatcher) : IOcDispatcher
{
    public void Invoke(Action action, int priority, object parameter, object context) =>
        dispatcher.Invoke(action, priority);

    [field: MaybeNull]
    public static AvaloniaOcDispatcher UIThread => field ??= new(Dispatcher.UIThread);
}

#endif