using System.Collections.Generic;
using Alba.Framework.Collections;

namespace Alba.Framework.Common
{
    public static class OwnedExts
    {
        public static IEnumerable<T> TraverseToRootOwner<T> (this T root) where T : class, IOwned
        {
            return root.TraverseList(i => i.Owner as T);
        }
    }
}