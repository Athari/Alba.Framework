using System.Collections.Specialized;
using System.Windows;

namespace Alba.Framework.Windows.Markup
{
    public class PushBindingCollection : FreezableCollection<PushBinding>
    {
        public PushBindingCollection ()
        {}

        public PushBindingCollection (DependencyObject targetObject)
        {
            TargetObject = targetObject;
            ((INotifyCollectionChanged)this).CollectionChanged += CollectionChanged;
        }

        public DependencyObject TargetObject { get; private set; }

        private void CollectionChanged (object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add) {
                foreach (PushBinding pushBinding in e.NewItems)
                    pushBinding.SetupTargetBinding(TargetObject);
            }
        }
    }
}