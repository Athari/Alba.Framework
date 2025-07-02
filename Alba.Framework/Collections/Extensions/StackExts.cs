namespace Alba.Framework.Collections;

public static class StackExts
{
    public static void PushRange<T>(this Stack<T> @this, IEnumerable<T> items)
    {
        foreach (T item in items)
            @this.Push(item);
    }
}