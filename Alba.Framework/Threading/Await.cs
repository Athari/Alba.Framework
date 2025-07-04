using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Alba.Framework.Threading;

[PublicAPI]
[SuppressMessage("ReSharper", "MethodOverloadWithOptionalParameter", Justification = "GetAwaiter method must have no parameters to be recognized")]
public static class Await
{
    public static YieldAwaitable CurrentYield { get; } = Task.Yield();
    public static TaskSchedulerAwaiter Default { get; } = TaskScheduler.Default.GetAwaiter(alwaysYield: false);
    public static TaskSchedulerAwaiter DefaultYield { get; } = TaskScheduler.Default.GetAwaiter(alwaysYield: true);

    public static TaskSchedulerAwaiter GetAwaiter(this TaskScheduler @this) =>
        new(@this, alwaysYield: false);

    public static TaskSchedulerAwaiter GetAwaiter(this TaskScheduler @this, bool alwaysYield = false) =>
        new(@this, alwaysYield);

    public static SynchronizationContextAwaiter GetAwaiter(this SynchronizationContext @this) =>
        new(@this);

    public static TaskSchedulerAwaiter GetAwaiter(this TaskFactory @this) =>
        new(@this.Scheduler ?? throw new ArgumentNullException(nameof(@this)));

    public static TaskSchedulerAwaiter GetAwaiter(this TaskFactory @this, bool alwaysYield = false) =>
        new(@this.Scheduler ?? throw new ArgumentNullException(nameof(@this)), alwaysYield);

    public static CancellationTokenAwaiter GetAwaiter(this CancellationToken @this) =>
        new(@this, useSynchronizationContext: true);

    public static CancellationTokenAwaiter GetAwaiter(this CancellationToken @this, bool useSynchronizationContext = true) =>
        new(@this, useSynchronizationContext);

    public static CancellationTokenAwaiter GetAwaiter(this CancellationTokenSource @this) =>
        new(@this.Token, useSynchronizationContext: true);

    public static CancellationTokenAwaiter GetAwaiter(this CancellationTokenSource @this, bool useSynchronizationContext = true) =>
        new(@this.Token, useSynchronizationContext);

    [PublicAPI]
    public readonly struct TaskSchedulerAwaiter(TaskScheduler scheduler, bool alwaysYield = false) : ICriticalNotifyCompletion
    {
        public bool IsCompleted =>
            !alwaysYield && (
                (scheduler == TaskScheduler.Default && Thread.CurrentThread.IsThreadPoolThread)
             || (scheduler == TaskScheduler.Current && TaskScheduler.Current != TaskScheduler.Default)
            );

        public void OnCompleted(Action continuation)
        {
            if (scheduler == TaskScheduler.Default)
                ThreadPool.QueueUserWorkItem(state => ((Action)state!)(), continuation);
            else
                Task.Factory.StartNew(continuation, CancellationToken.None, TaskCreationOptions.None, scheduler);
        }

        public void UnsafeOnCompleted(Action continuation)
        {
            if (scheduler == TaskScheduler.Default)
                ThreadPool.UnsafeQueueUserWorkItem(state => ((Action)state!)(), continuation);
            else
                Task.Factory.StartNew(continuation, CancellationToken.None, TaskCreationOptions.None, scheduler);
        }

        public TaskSchedulerAwaiter GetAwaiter() => this;

        public void GetResult() { }
    }

    [PublicAPI]
    public readonly struct SynchronizationContextAwaiter(SynchronizationContext syncContext) : ICriticalNotifyCompletion
    {
        private static readonly SendOrPostCallback SyncContextDelegate = state => ((Action)state!)();
        public bool IsCompleted => false;
        public void OnCompleted(Action continuation) => syncContext.Post(SyncContextDelegate, continuation);
        public void UnsafeOnCompleted(Action continuation) => syncContext.Post(SyncContextDelegate, continuation);
        public SynchronizationContextAwaiter GetAwaiter() => this;
        public void GetResult() { }
    }

    [PublicAPI]
    public readonly struct CancellationTokenAwaiter(CancellationToken token, bool useSynchronizationContext = true) : ICriticalNotifyCompletion
    {
        public bool IsCompleted => token.IsCancellationRequested;
        public void OnCompleted(Action continuation) => token.Register(continuation, useSynchronizationContext);
        public void UnsafeOnCompleted(Action continuation) => token.Register(continuation, useSynchronizationContext);
        public CancellationTokenAwaiter GetAwaiter() => this;

        public object GetResult() =>
            throw (IsCompleted
                ? new OperationCanceledException()
                : new InvalidOperationException("The cancellation token has not yet been cancelled."));
    }
}