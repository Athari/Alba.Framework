using System.Collections.ObjectModel;

namespace Alba.Framework.Collections;

[PublicAPI]
public static class ObservableCollectionExts
{
    public static ReadOnlyObservableCollection<T> ToReadOnlyObservable<T>(this ObservableCollection<T> @this) =>
        new(@this);
}