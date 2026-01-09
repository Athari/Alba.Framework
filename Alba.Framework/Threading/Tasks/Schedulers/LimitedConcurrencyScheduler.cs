namespace Alba.Framework.Threading.Tasks.Schedulers;

/// <summary>
/// Provides a task scheduler that ensures a maximum concurrency level while running on top of the ThreadPool.
/// Source: http://code.msdn.microsoft.com/ParExtSamples
/// Documentation: http://blogs.msdn.com/b/pfxteam/archive/2010/04/09/9990424.aspx
/// License: MS-LPL
/// </summary>
public class LimitedConcurrencyScheduler : TaskScheduler
{
    [ThreadStatic]
    private static bool _CurrentThreadIsProcessingItems;

    private readonly LinkedList<Task> _tasks = [ ]; // protected by lock(_tasks)
    private int _delegatesQueuedOrRunning = 0; // protected by lock(_tasks)

    /// <summary>Gets the maximum concurrency level supported by this scheduler.</summary>
    public sealed override int MaximumConcurrencyLevel { get; }

    /// <summary>Initializes an instance of the LimitedConcurrencyLevelTaskScheduler class with the specified degree of parallelism.</summary>
    /// <param name="maxDegreeOfParallelism">The maximum degree of parallelism provided by this scheduler.</param>
    public LimitedConcurrencyScheduler(int maxDegreeOfParallelism)
    {
        Guard.IsGreaterThan(maxDegreeOfParallelism, 0);
        MaximumConcurrencyLevel = maxDegreeOfParallelism;
    }

    protected sealed override IEnumerable<Task> GetScheduledTasks()
    {
        bool lockTaken = false;
        try {
            Monitor.TryEnter(_tasks, ref lockTaken);
            if (lockTaken)
                return [ .. _tasks ];
            else
                throw new NotSupportedException();
        }
        finally {
            if (lockTaken)
                Monitor.Exit(_tasks);
        }
    }

    protected sealed override void QueueTask(Task task)
    {
        // Add the task to the list of tasks to be processed.  If there aren't enough
        // delegates currently queued or running to process tasks, schedule another.
        lock (_tasks) {
            _tasks.AddLast(task);
            if (_delegatesQueuedOrRunning < MaximumConcurrencyLevel) {
                ++_delegatesQueuedOrRunning;
                NotifyThreadPoolOfPendingWork();
            }
        }
    }

    /// <summary>Informs the ThreadPool that there's work to be executed for this scheduler.</summary>
    private void NotifyThreadPoolOfPendingWork()
    {
        ThreadPool.UnsafeQueueUserWorkItem(_ => {
            // Note that the current thread is now processing work items.
            // This is necessary to enable inlining of tasks into this thread.
            _CurrentThreadIsProcessingItems = true;
            try {
                // Process all available items in the queue.
                while (true) {
                    Task item;
                    lock (_tasks) {
                        // When there are no more items to be processed,
                        // note that we're done processing, and get out.
                        if (_tasks.Count == 0) {
                            --_delegatesQueuedOrRunning;
                            break;
                        }
                        // Get the next item from the queue
                        item = _tasks.First!.Value;
                        _tasks.RemoveFirst();
                    }
                    // Execute the task we pulled out of the queue
                    TryExecuteTask(item);
                }
            }
            finally {
                // We're done processing items on the current thread
                _CurrentThreadIsProcessingItems = false;
            }
        }, null);
    }

    protected sealed override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
    {
        // If this thread isn't already processing a task, we don't support inlining
        if (!_CurrentThreadIsProcessingItems)
            return false;
        // If the task was previously queued, remove it from the queue
        if (taskWasPreviouslyQueued)
            TryDequeue(task);
        // Try to run the task.
        return TryExecuteTask(task);
    }

    protected sealed override bool TryDequeue(Task task)
    {
        lock (_tasks)
            return _tasks.Remove(task);
    }
}