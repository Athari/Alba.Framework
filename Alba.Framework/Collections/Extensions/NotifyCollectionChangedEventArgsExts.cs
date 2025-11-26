using System.Collections.Specialized;

namespace Alba.Framework.Collections;

public static class NotifyCollectionChangedEventArgsExts
{
    extension(NotifyCollectionChangedEventArgs @this)
    {
        public IEnumerable<T> GetNewItems<T>() => @this.NewItems?.Cast<T>() ?? [ ];
        public IEnumerable<T> GetOldItems<T>() => @this.OldItems?.Cast<T>() ?? [ ];
    }
}