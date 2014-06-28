using System.ComponentModel;
using System.Windows;
using System.Windows.Data;

namespace Alba.Framework.Windows.Markup
{
    /// <summary>
    /// Helper for OneWayToSource binding for read-only dependency properties.
    /// Based on code from http://meleak.wordpress.com/2011/08/28/onewaytosource-binding-for-readonly-dependency-property/
    /// </summary>
    public class PushBinding : FreezableBinding
    {
        public static readonly DependencyProperty TargetPropertyMirrorProperty = DependencyProperty.Register(
            "TargetPropertyMirror", typeof(object), typeof(PushBinding));
        public static readonly DependencyProperty TargetPropertyListenerProperty = DependencyProperty.Register(
            "TargetPropertyListener", typeof(object), typeof(PushBinding),
            new PropertyMetadata(null, (d, e) => ((PushBinding)d).OnTargetPropertyListenerPropertyChanged()));
        public static readonly DependencyProperty BindingsProperty = DependencyProperty.RegisterAttached(
            "BindingsInternal", typeof(PushBindingCollection), typeof(PushBinding));
        public static readonly DependencyProperty StyleBindingsProperty = DependencyProperty.RegisterAttached(
            "StyleBindings", typeof(PushBindingCollection), typeof(PushBinding),
            new PropertyMetadata(null, (d, e) => ((PushBinding)d).OnStyleBindingsPropertyChanged(e)));

        public PushBinding ()
        {
            Mode = BindingMode.OneWayToSource;
        }

        [DefaultValue (null)]
        public string TargetProperty { get; set; }

        [DefaultValue (null)]
        public DependencyProperty TargetDependencyProperty { get; set; }

        public object TargetPropertyMirror
        {
            get { return GetValue(TargetPropertyMirrorProperty); }
            set { SetValue(TargetPropertyMirrorProperty, value); }
        }

        public object TargetPropertyListener
        {
            get { return GetValue(TargetPropertyListenerProperty); }
            set { SetValue(TargetPropertyListenerProperty, value); }
        }

        public static PushBindingCollection GetBindings (DependencyObject obj)
        {
            if (obj.GetValue(BindingsProperty) == null)
                obj.SetValue(BindingsProperty, new PushBindingCollection(obj));
            return (PushBindingCollection)obj.GetValue(BindingsProperty);
        }

        public static void SetBindings (DependencyObject obj, PushBindingCollection value)
        {
            obj.SetValue(BindingsProperty, value);
        }

        public static PushBindingCollection GetStyleBindings (DependencyObject obj)
        {
            return (PushBindingCollection)obj.GetValue(StyleBindingsProperty);
        }

        public static void SetStyleBindings (DependencyObject obj, PushBindingCollection value)
        {
            obj.SetValue(StyleBindingsProperty, value);
        }

        internal void SetupTargetBinding (DependencyObject target)
        {
            if (target == null)
                return;

            // Prevent the designer from reporting exceptions since
            // changes will be made of a Binding in use if it is set
            if (DesignerProperties.GetIsInDesignMode(this))
                return;

            // Bind to the selected TargetProperty, e.g. ActualHeight and get
            // notified about changes in OnTargetPropertyListenerChanged
            Binding listenerBinding = new Binding {
                Source = target,
                Mode = BindingMode.OneWay,
                Path = TargetDependencyProperty != null ? new PropertyPath(TargetDependencyProperty) : new PropertyPath(TargetProperty)
            };
            BindingOperations.SetBinding(this, TargetPropertyListenerProperty, listenerBinding);

            // Set up a OneWayToSource Binding with the Binding declared in Xaml from
            // the Mirror property of this class. The mirror property will be updated
            // everytime the Listener property gets updated
            BindingOperations.SetBinding(this, TargetPropertyMirrorProperty, Binding);

            OnTargetPropertyListenerPropertyChanged();
            var targetElement = target as FrameworkElement;
            if (targetElement != null)
                targetElement.Loaded += delegate { OnTargetPropertyListenerPropertyChanged(); };
            var targetContentElement = target as FrameworkContentElement;
            if (targetContentElement != null)
                targetContentElement.Loaded += delegate { OnTargetPropertyListenerPropertyChanged(); };
        }

        private void OnStyleBindingsPropertyChanged (DependencyPropertyChangedEventArgs e)
        {
            PushBindingCollection pushBindingCollection = GetBindings(this);
            foreach (PushBinding pushBinding in (PushBindingCollection)e.NewValue)
                pushBindingCollection.Add((PushBinding)pushBinding.Clone());
        }

        private void OnTargetPropertyListenerPropertyChanged ()
        {
            SetValue(TargetPropertyMirrorProperty, GetValue(TargetPropertyListenerProperty));
        }

        protected override void CloneCore (Freezable sourceFreezable)
        {
            var source = (PushBinding)sourceFreezable;
            TargetProperty = source.TargetProperty;
            TargetDependencyProperty = source.TargetDependencyProperty;
            base.CloneCore(sourceFreezable);
        }

        protected override Freezable CreateInstanceCore ()
        {
            return new PushBinding();
        }
    }
}