#if OC_SUPPORT_AVALONIA

using Avalonia.Threading;
using ObservableComputations;

namespace Alba.Framework.Collections.ObservableComputations;

public class AvaloniaOcDispatcher(Dispatcher dispatcher) : IOcDispatcher
{
    public void Invoke(Action action, int priority, object parameter, object context) =>
        dispatcher.Invoke(action, priority);

    [field: MaybeNull]
    public static AvaloniaOcDispatcher UIThread => field ??= new(Dispatcher.UIThread);
}

#endif