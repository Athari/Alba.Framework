using System.Collections;

namespace Alba.Framework.Collections;

[PublicAPI]
public static class EnumeratorExts
{
    public static IEnumerator<T> Cast<T>(this IEnumerator @this)
    {
        while (@this.MoveNext())
            yield return (T)@this.Current!;
    }

    public static IEnumerable ToEnumerable(this IEnumerator @this)
    {
        while (@this.MoveNext())
            yield return @this.Current;
    }

    public static IEnumerable ToEnumerable<T, TEnum>(this TEnum @this, Func<TEnum, T> getItem)
        where TEnum : IEnumerator
    {
        while (@this.MoveNext())
            yield return getItem(@this);
    }
}