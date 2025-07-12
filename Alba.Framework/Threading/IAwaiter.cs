using System.Runtime.CompilerServices;

namespace Alba.Framework.Threading;

/// <summary>Strict typing for duck-typed awaiters.</summary>
[PublicAPI]
public interface IAwaiterBase<out TSelf> : ICriticalNotifyCompletion
{
    bool IsCompleted { get; }

    TSelf GetAwaiter();
}

[PublicAPI]
public interface IAwaiter<out TSelf, out TResult> : IAwaiterBase<TSelf>
{
    TResult GetResult();
}

[PublicAPI]
public interface IAwaiter<out TSelf> : IAwaiterBase<TSelf>
{
    void GetResult();
}