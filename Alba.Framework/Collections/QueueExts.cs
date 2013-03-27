using System.Collections.Generic;

namespace Alba.Framework.Collections
{
    public static class QueueExts
    {
        public static void EnqueueRange<T> (this Queue<T> @this, IEnumerable<T> items)
        {
            foreach (T item in items)
                @this.Enqueue(item);
        }
    }
}