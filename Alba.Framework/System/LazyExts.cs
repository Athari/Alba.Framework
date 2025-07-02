namespace Alba.Framework;

[PublicAPI]
public static class LazyExts
{
    public static Lazy<T> Force<T>(this Lazy<T> @this)
    {
        _ = @this.Value;
        return @this;
    }
}