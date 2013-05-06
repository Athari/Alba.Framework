using System.Collections;
using System.Collections.Generic;

namespace Alba.Framework.Collections
{
    public static class EnumeratorExts
    {
        public static IEnumerator<T> Cast<T> (this IEnumerator @this)
        {
            while (@this.MoveNext())
                yield return (T)@this.Current;
        }
    }
}