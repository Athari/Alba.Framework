using System.ComponentModel;

namespace Alba.Framework.Collections
{
    public interface INotifyCollectionItemChanged
    {
        event PropertyChangingEventHandler ItemPropertyChanging;
        event PropertyChangedEventHandler ItemPropertyChanged;
    }
}