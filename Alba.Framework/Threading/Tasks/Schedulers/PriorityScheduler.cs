using System.Collections.Concurrent;
using Alba.Framework.Collections;

namespace Alba.Framework.Threading.Tasks.Schedulers;

public class PriorityScheduler : TaskScheduler, IDisposable
{
    public static PriorityScheduler Lowest => field ??= new(ThreadPriority.Lowest);
    public static PriorityScheduler BelowNormal => field ??= new(ThreadPriority.BelowNormal);
    public static PriorityScheduler AboveNormal => field ??= new(ThreadPriority.AboveNormal);
    public static PriorityScheduler Highest => field ??= new(ThreadPriority.Highest);

    private readonly ThreadPriority _priority;
    private readonly int _maximumConcurrency;
    private readonly BlockingCollection<Task> _tasks = new();
    private readonly Lazy<Thread[]> _threads;

    public PriorityScheduler(ThreadPriority priority, int? maximumConcurrency = null)
    {
        _priority = priority;
        _maximumConcurrency = Math.Max(1, maximumConcurrency ?? Environment.ProcessorCount);
        _threads = new(CreateThreads);
    }

    public override int MaximumConcurrencyLevel => _maximumConcurrency;

    protected override IEnumerable<Task> GetScheduledTasks() => _tasks;

    protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued) => false;

    protected override void QueueTask(Task task)
    {
        _tasks.Add(task);
        _ = _threads.Value;
    }

    private Thread[] CreateThreads() =>
        Enumerable
            .Range(0, _maximumConcurrency)
            .Select(i => new Thread(ThreadStart) {
                Name = $"{nameof(PriorityScheduler)} {_priority} {i}",
                Priority = _priority,
                IsBackground = true,
            })
            .Do(t => t.Start())
            .ToArray();

    private void ThreadStart()
    {
        foreach (var t in _tasks.GetConsumingEnumerable())
            TryExecuteTask(t);
    }

    public void Dispose()
    {
        _tasks.CompleteAdding();
        _tasks.Dispose();
        GC.SuppressFinalize(this);
    }
}