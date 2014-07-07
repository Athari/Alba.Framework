using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace Alba.Framework.Caliburn
{
    public class KeyTrigger : TriggerBase<UIElement>
    {
        private static readonly KeyGestureConverter _keyGestureConverter = new KeyGestureConverter();

        public static readonly DependencyProperty KeyGestureProperty = DependencyProperty.Register(
            "KeyGesture", typeof(KeyGesture), typeof(KeyTrigger), null);

        public KeyTrigger ()
        {}

        public KeyTrigger (KeyGesture keyGesture)
        {
            KeyGesture = keyGesture;
        }

        public KeyTrigger (string keyGesture)
        {
            KeyGesture = (KeyGesture)_keyGestureConverter.ConvertFromString(keyGesture);
        }

        public KeyGesture KeyGesture
        {
            get { return (KeyGesture)GetValue(KeyGestureProperty); }
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