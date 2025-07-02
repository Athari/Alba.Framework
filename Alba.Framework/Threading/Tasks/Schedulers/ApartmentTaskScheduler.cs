using System.Collections.Concurrent;
using System.Runtime.Versioning;
using Alba.Framework.Collections;

#pragma warning disable IDE0305 // Use collection expression for fluent - messes up LINQ chain
namespace Alba.Framework.Threading.Tasks.Schedulers;

/// <summary>
/// Provides a scheduler that uses specified number of threads (not from thread pool), optionally STA.
/// Based on StaTaskScheduler.
/// Source: http://code.msdn.microsoft.com/ParExtSamples
/// Documentation: http://blogs.msdn.com/b/pfxteam/archive/2010/04/09/9990424.aspx
/// License: MS-LPL
/// </summary>
[SupportedOSPlatform("windows")]
public class ApartmentTaskScheduler : TaskScheduler, IDisposable
{
    private readonly BlockingCollection<Task> _tasks;
    private readonly List<Thread> _threads;
    private readonly ApartmentState _apartmentState;
    private bool _isDisposed;

    /// <summary>Initializes a new instance of the StaTaskScheduler class with specified number of threads.</summary>
    /// <param name="numberOfThreads">The number of threads that should be created and used by this scheduler.</param>
    /// <param name="apartmentState">Apartment state of created threads.</param>
    /// <param name="threadName">Base name for threads.</param>
    public ApartmentTaskScheduler(int numberOfThreads = 1, ApartmentState apartmentState = ApartmentState.MTA, string threadName = "ApartmentTaskScheduler")
    {
        Guard.IsGreaterThan(numberOfThreads, 0, nameof(numberOfThreads));

        _apartmentState = apartmentState;
        _tasks = [ ];
        _threads = numberOfThreads.Range().Select(i => {
            var thread = new Thread(ThreadStart) {
                IsBackground = true,
                Name = numberOfThreads == 1 ? threadName : $"{threadName} #{i}",
            };
            thread.SetApartmentState(apartmentState);
            return thread;
        }).ToList();

        foreach (Thread thread in _threads)
            thread.Start();
    }

    private void ThreadStart()
    {
        // Continually get the next task and try to execute it.
        // This will continue until the scheduler is disposed and no more tasks remain.
        foreach (Task t in _tasks.GetConsumingEnumerable())
            TryExecuteTask(t);
    }

    /// <summary>Gets the maximum concurrency level supported by this scheduler.</summary>
    public override int MaximumConcurrencyLevel
    {
        get { return _threads.Count; }
    }

    protected override IEnumerable<Task> GetScheduledTasks()
    {
        return [ .. _tasks ];
    }

    protected override void QueueTask(Task task)
    {
        _tasks.Add(task);
    }

    protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
    {
        return Thread.CurrentThread.GetApartmentState() == _apartmentState && TryExecuteTask(task);
    }

    /// <summary>
    /// Cleans up the scheduler by indicating that no more tasks will be queued.
    /// This method blocks until all threads successfully shutdown.
    /// </summary>
    public void Dispose()
    {
        if (_isDisposed)
            return;

        // Indicate that no new tasks will be coming in
        _tasks.CompleteAdding();

        // Wait for all threads to finish processing tasks
        foreach (var thread in _threads)
            thread.Join();

        // Cleanup
        _tasks.Dispose();
        _isDisposed = true;
        GC.SuppressFinalize(this);
    }
}