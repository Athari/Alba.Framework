using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Transactions;

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

    public static WaitHandleAwaiter GetAwaiter(this WaitHandle @this) =>
        new(@this);

    public static WaitHandleAwaiter GetAwaiter(this CommittableTransaction @this) =>
        new((@this as IAsyncResult).AsyncWaitHandle);

    public static WaitHandleAwaiter GetAwaiter(this CountdownEvent @this) =>
        new(@this.WaitHandle);

    public static WaitHandleAwaiter GetAwaiter(this ManualResetEventSlim @this) =>
        new(@this.WaitHandle);

    public static WaitHandleAwaiter GetAwaiter(this SemaphoreSlim @this) =>
        new(@this.AvailableWaitHandle);

    public static WaitHandleAwaiter GetAwaiter(this IAsyncResult @this) =>
        new(@this.AsyncWaitHandle);

    public static TaskAwaiter<int> GetAwaiter(this Process process)
    {
        var tcs = new TaskCompletionSource<int>();
        process.EnableRaisingEvents = true;
        process.Exited += (_, _) => tcs.TrySetResult(process.ExitCode);
        if (process.HasExited)
            tcs.TrySetResult(process.ExitCode);
        return tcs.Task.GetAwaiter();
    }

    [PublicAPI]
    public readonly struct TaskSchedulerAwaiter(TaskScheduler scheduler, bool alwaysYield = false)
        : IAwaiter<TaskSchedulerAwaiter>
    {
        public bool IsCompleted =>
            !alwaysYield && (
                (scheduler == TaskScheduler.Default && Thread.CurrentThread.IsThreadPoolThread)
             || (scheduler == TaskScheduler.Current && TaskScheduler.Current != TaskScheduler.Default)
            );

        public void OnCompleted(Action continuation)
        {
            if (scheduler == TaskScheduler.Default)
                ThreadPool.QueueUserWorkItem(Execute, continuation);
            else
                Task.Factory.StartNew(continuation, CancellationToken.None, TaskCreationOptions.None, scheduler);
        }

        public void UnsafeOnCompleted(Action continuation)
        {
            if (scheduler == TaskScheduler.Default)
                ThreadPool.UnsafeQueueUserWorkItem(Execute, continuation);
            else
                Task.Factory.StartNew(continuation, CancellationToken.None, TaskCreationOptions.None, scheduler);
        }

        public TaskSchedulerAwaiter GetAwaiter() => this;

        public void GetResult() { }
    }

    [PublicAPI]
    public readonly struct SynchronizationContextAwaiter(SynchronizationContext syncContext)
        : IAwaiter<SynchronizationContextAwaiter>
    {
        public bool IsCompleted => false;

        public void OnCompleted(Action continuation) =>
            syncContext.Post(Execute, continuation);

        public void UnsafeOnCompleted(Action continuation) =>
            syncContext.Post(Execute, continuation);

        public SynchronizationContextAwaiter GetAwaiter() => this;

        public void GetResult() { }
    }

    [PublicAPI]
    public readonly struct CancellationTokenAwaiter(CancellationToken token, bool useSynchronizationContext = true)
        : IAwaiter<CancellationTokenAwaiter, object>
    {
        public bool IsCompleted => token.IsCancellationRequested;

        public void OnCompleted(Action continuation)
        {
            if (token.CanBeCanceled)
                token.Register(continuation, useSynchronizationContext);
        }

        public void UnsafeOnCompleted(Action continuation)
        {
            if (token.CanBeCanceled)
                token.UnsafeRegister(Execute, continuation);
        }

        public CancellationTokenAwaiter GetAwaiter() => this;

        public object GetResult() =>
            throw (IsCompleted
                ? new OperationCanceledException()
                : new InvalidOperationException("The cancellation token has not yet been cancelled."));
    }

    public readonly struct WaitHandleAwaiter(WaitHandle handle) : IAwaiter<WaitHandleAwaiter>
    {
        public bool IsCompleted => handle.WaitOne(0);

        public void OnCompleted(Action continuation) =>
            ThreadPool.RegisterWaitForSingleObject(handle, Execute, WithExecutionContext(continuation), Timeout.Infinite, true);

        public void UnsafeOnCompleted(Action continuation) =>
            ThreadPool.UnsafeRegisterWaitForSingleObject(handle, Execute, continuation, Timeout.Infinite, true);

        public WaitHandleAwaiter GetAwaiter() => this;

        public void GetResult() { }
    }

    private static Action WithExecutionContext(Action continuation)
    {
        var context = ExecutionContext.Capture();
        return context != null ? () => ExecutionContext.Run(context, Execute, continuation) : continuation;
    }

    private static void Execute(object? state) => ((Action)state!)();
    private static void Execute(object? state, bool _ /*timedOut*/) => ((Action)state!)();
}