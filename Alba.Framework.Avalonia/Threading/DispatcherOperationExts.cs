using Alba.Framework.Threading.Tasks;
using Avalonia.Threading;

namespace Alba.Framework.Avalonia.Threading;

public static class DispatcherOperationExts
{
    public static void NoAwait(this DispatcherOperation @this, Action<Exception>? onFaulted = null) =>
        @this.GetTask().NoWait(onFaulted);

    public static void NoAwait<T>(this DispatcherOperation<T> @this, Action<Exception>? onFaulted = null) =>
        @this.GetTask().NoWait(onFaulted);
}