using System.Collections;

namespace Alba.Framework.Collections;

[PublicAPI]
public static class EnumeratorExts
{
    extension(IEnumerator @this)
    {
        public IEnumerator<T> Cast<T>()
        {
            while (@this.MoveNext())
                yield return (T)@this.Current!;
        }

        public IEnumerable ToEnumerable()
        {
            while (@this.MoveNext())
                yield return @this.Current;
        }
    }

    extension<TEnumerator>(TEnumerator @this) where TEnumerator : IEnumerator
    {
        public IEnumerable ToEnumerable<T>(Func<TEnumerator, T> selector)
        {
            while (@this.MoveNext())
                yield return selector(@this);
        }
    }
}