using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;
using Alba.Framework.ComponentModel;
using Alba.Framework.Windows.Input;

namespace Alba.Framework.Caliburn
{
    public class KeyTrigger : TriggerBase<UIElement>
    {
        private static readonly TypeConverter _keyGestureConverter = new AnyKeyGestureConverter();

        public static readonly DependencyProperty KeyGestureProperty = DependencyProperty.Register(
            "KeyGesture", typeof(AnyKeyGesture), typeof(KeyTrigger), null);

        public KeyTrigger ()
        {}

        public KeyTrigger (AnyKeyGesture keyGesture)
        {
            KeyGesture = keyGesture;
        }

        public KeyTrigger (string keyGesture)
        {
            KeyGesture = _keyGestureConverter.ConvertFromString<AnyKeyGesture>(keyGesture);
        }

        public AnyKeyGesture KeyGesture
        {
            get { return (AnyKeyGesture)GetValue(KeyGestureProperty); }
            set { SetValue(KeyGestureProperty, value); }
        }

        protected override void OnAttached ()
        {
            base.OnAttached();
            AssociatedObject.KeyDown += OnAssociatedObjectKeyDown;
        }

        protected override void OnDetaching ()
        {
            base.OnDetaching();
            AssociatedObject.KeyDown -= OnAssociatedObjectKeyDown;
        }

        private void OnAssociatedObjectKeyDown (object sender, KeyEventArgs e)
        {
            if (KeyGesture.Matches(sender, e))
                InvokeActions(e);
        }
    }
}