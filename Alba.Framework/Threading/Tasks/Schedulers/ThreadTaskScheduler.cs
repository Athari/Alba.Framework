using System.Threading;

namespace Alba.Framework.Threading.Tasks.Schedulers
{
    /// <summary>
    /// Provides a task scheduler that ensures only one task is executing at a time, and that tasks
    /// execute in the order that they were queued. Uses a single thread.
    /// </summary>
    public sealed class ThreadTaskScheduler : ApartmentTaskScheduler
    {
        public ThreadTaskScheduler (ApartmentState apartmentState = ApartmentState.MTA, string threadName = "ThreadTaskScheduler")
            : base(1, apartmentState, threadName)
        {}
    }
}