namespace Alba.Framework.Collections;

public static class QueueExts
{
    extension<T>(Queue<T> @this)
    {
        public void EnqueueRange(IEnumerable<T> items)
        {
            foreach (T item in items)
                @this.Enqueue(item);
        }
    }
}