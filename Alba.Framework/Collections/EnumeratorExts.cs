using System.Collections;
using System.Collections.Generic;

namespace Alba.Framework.Collections
{
    public static class EnumeratorExts
    {
        public static IEnumerator<T> ToTyped<T> (this IEnumerator @this)
        {
            return new TypedEnumerator<T>(@this);
        }
    }
}