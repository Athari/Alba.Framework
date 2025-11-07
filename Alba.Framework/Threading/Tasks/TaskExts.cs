using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;

namespace Alba.Framework.Threading.Tasks;

[PublicAPI]
public static class TaskExts
{
    public static ConfiguredTaskAwaitable NoSync(this Task @this) =>
        @this.ConfigureAwait(false);

    public static ConfiguredTaskAwaitable<T> NoSync<T>(this Task<T> @this) =>
        @this.ConfigureAwait(false);

    public static ConfiguredAsyncDisposable NoSync<T>(this T @this) where T : IAsyncDisposable =>
        @this.ConfigureAwait(false);

    public static ConfiguredAsyncDisposable AsNoSync<T>(this T @this, out T var) where T : IAsyncDisposable =>
        @this.As(out var).ConfigureAwait(false);

    public static ConfiguredValueTaskAwaitable NoSync(this ValueTask @this) =>
        @this.ConfigureAwait(false);

    public static ConfiguredValueTaskAwaitable<T> NoSync<T>(this ValueTask<T> @this) =>
        @this.ConfigureAwait(false);

    public static void ContinueOnFaultedWith(this Task @this, Action<Task, AggregateException> onFaulted) =>
        @this.ContinueWith(t => onFaulted(t, t.Exception!), TaskContinuationOptions.OnlyOnFaulted);

    public static void ContinueOnFaultedWith<T>(this Task<T> @this, Action<Task, AggregateException> onFaulted) =>
        @this.ContinueWith(t => onFaulted(t, t.Exception!), TaskContinuationOptions.OnlyOnFaulted);

    public static void ContinueOnCanceledWith(this Task @this, Action<Task> onCanceled) =>
        @this.ContinueWith(onCanceled, TaskContinuationOptions.OnlyOnCanceled);

    public static void ContinueOnCanceledWith<T>(this Task<T> @this, Action<Task> onCanceled) =>
        @this.ContinueWith(onCanceled, TaskContinuationOptions.OnlyOnCanceled);

    public static void NoAwait(this Task @this, Action<Exception>? onFaulted = null) =>
        @this.ContinueOnFaultedWith((_, e) => onFaulted?.Invoke(e.InnerException ?? e));

    public static void NoAwait<T>(this Task<T> @this, Action<Exception>? onFaulted = null) =>
        @this.ContinueOnFaultedWith((_, e) => onFaulted?.Invoke(e.InnerException ?? e));

    public static void NoAwait(this ValueTask _) { }

    public static void NoAwait<T>(this ValueTask<T> _) { }

    public static Task OrCompleted(this Task? @this) =>
        @this ?? Task.CompletedTask;

    public static Task<T> OrCompleted<T>(this Task<T>? @this, T defaultValue = default!) =>
        @this ?? Task.FromResult(defaultValue);

    public static ValueTask OrCompleted(this ValueTask? @this) =>
        @this ?? ValueTask.CompletedTask;

    public static ValueTask<T> OrCompleted<T>(this ValueTask<T>? @this, T defaultValue = default!) =>
        @this ?? ValueTask.FromResult(defaultValue);

    public static IEnumerable<T> Yield<T>(this T @this)
    {
        yield return @this;
    }

    /// <summary>Aggregates exceptions from all tasks into a single <see cref="AggregateException"/>.</summary>
    public static async Task WhenAllSafe(params IEnumerable<Task> tasks)
    {
        var whenAllTask = Task.WhenAll(tasks ?? throw new ArgumentNullException(nameof(tasks)));
        try {
            await whenAllTask;
        }
        catch {
            if (whenAllTask.Exception != null)
                ExceptionDispatchInfo.Capture(whenAllTask.Exception).Throw();
            throw; // TaskCancelledException
        }
    }

    /// <summary>Aggregates exceptions from all tasks into a single <see cref="AggregateException"/>.</summary>
    public static async Task<TResult[]> WhenAllSafe<TResult>(params IEnumerable<Task<TResult>> tasks)
    {
        var whenAllTask = Task.WhenAll(tasks);
        try {
            return await whenAllTask;
        }
        catch {
            if (whenAllTask.Exception != null)
                ExceptionDispatchInfo.Capture(whenAllTask.Exception).Throw();
            throw; // TaskCancelledException
        }
    }

    public static Task WithTimeoutAsync(this Task task, TimeSpan timeout, CancellationToken ct = default)
    {
        return WithTimeoutAsync(async _ => {
            await task;
            return Unit.Void;
        }, timeout, ct);
    }

    public static async Task<T> WithTimeoutAsync<T>(this Task<T> task, TimeSpan timeout, CancellationToken ct = default)
    {
        await WithTimeoutAsync(_ => task, timeout, ct);
        return await task;
    }

    /// <remarks>From https://github.com/microsoft/BuildXL/</remarks>
    public static async Task<T> WithTimeoutAsync<T>(Func<CancellationToken, Task<T>> taskFactory, TimeSpan timeout, CancellationToken ct = default)
    {
        if (timeout == Timeout.InfiniteTimeSpan)
            return await taskFactory(ct);

        using var timeoutCts = new CancellationTokenSource(timeout);
        using var cts = CancellationTokenSource.CreateLinkedTokenSource(timeoutCts.Token, ct);
        var task = taskFactory(cts.Token);
        await Task.WhenAny(task, Task.Delay(Timeout.Infinite, cts.Token));

        if (task.IsCompleted && (!task.IsCanceled || !timeoutCts.IsCancellationRequested))
            return await task;

        if (!ct.IsCancellationRequested)
            ObserveTaskAndThrow();
        await Task.WhenAny(task, Task.Delay(Timeout.Infinite, timeoutCts.Token));
        if (!task.IsCompleted && timeoutCts.IsCancellationRequested)
            ObserveTaskAndThrow();

        return await task;

        void ObserveTaskAndThrow()
        {
            task.NoAwait();
            throw new TimeoutException($"The operation has timed out. Timeout is '{timeout}'.");
        }
    }

    private readonly struct Unit
    {
        public static readonly Unit Void = default;
        //public static readonly Task<Unit> VoidTask = Task.FromResult(Void);
    }
}