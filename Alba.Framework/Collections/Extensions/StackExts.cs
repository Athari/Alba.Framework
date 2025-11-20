namespace Alba.Framework.Collections;

public static class StackExts
{
    extension<T>(Stack<T> @this)
    {
        public void PushRange(IEnumerable<T> items)
        {
            foreach (T item in items)
                @this.Push(item);
        }
    }
}