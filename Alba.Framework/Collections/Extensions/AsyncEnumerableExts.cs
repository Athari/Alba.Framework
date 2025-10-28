namespace Alba.Framework.Collections;

public static class AsyncEnumerableExts
{
    extension<T>(IAsyncEnumerable<T?> @this)
    {
        public IAsyncEnumerable<T> WhereNotNull()
        {
            return @this.Where(i => i != null)!;
        }
    }
}