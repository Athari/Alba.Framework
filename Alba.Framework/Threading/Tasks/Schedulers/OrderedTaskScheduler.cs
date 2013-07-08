namespace Alba.Framework.Threading.Tasks.Schedulers
{
    /// <summary>
    /// Provides a task scheduler that ensures only one task is executing at a time, and that tasks
    /// execute in the order that they were queued. Uses threads from ThreadPool.
    /// </summary>
    public sealed class OrderedTaskScheduler : LimitedConcurrencyTaskScheduler
    {
        /// <summary>Initializes an instance of the OrderedTaskScheduler class.</summary>
        public OrderedTaskScheduler () : base(1)
        {}
    }
}