using System.Collections.ObjectModel;

namespace Alba.Framework.Collections;

public static class ObservableCollectionExts
{
    extension<T>(ObservableCollection<T> @this)
    {
        public ReadOnlyObservableCollection<T> ToReadOnlyObservable() =>
            new(@this);
    }
}