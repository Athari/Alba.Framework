namespace Alba.Framework;

public static class WeakReferenceExts
{
    extension<T>(WeakReference<T> @this) where T : class
    {
        // Fuck "avoiding common pitfalls", forced TryGetTarget ruins pattern matching
        public T? Target => @this.TryGetTarget(out var target) ? target : null;
    }
}