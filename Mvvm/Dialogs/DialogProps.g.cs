using System.Windows;
using System.Windows.Controls.Primitives;
using Alba.Framework.Events;

// ReSharper disable RedundantCast
namespace Alba.Framework.Mvvm.Dialogs
{
    public static partial class DialogProps
    {
        public static DependencyProperty WindowButtonsProperty = DependencyProperty.RegisterAttached(
            "WindowButtons", typeof(WindowButton), typeof(DialogProps),
            new PropertyMetadata((WindowButton)WindowButton.Default, WindowButtons_Changed));
        public static DependencyProperty DialogButtonsProperty = DependencyProperty.RegisterAttached(
            "DialogButtons", typeof(DialogButton), typeof(DialogProps),
            new PropertyMetadata(default(DialogButton)));
        public static DependencyProperty ButtonResultProperty = DependencyProperty.RegisterAttached(
            "ButtonResult", typeof(DialogButton), typeof(DialogProps),
            new PropertyMetadata(default(DialogButton), ButtonResult_Changed));
        public static DependencyProperty DialogResultProperty = DependencyProperty.RegisterAttached(
            "DialogResult", typeof(DialogButton), typeof(DialogProps),
            new PropertyMetadata((DialogButton)DialogButton.Cancel));
        public static WindowButton GetWindowButtons (Window d)
        {
            return (WindowButton)d.GetValue(WindowButtonsProperty);
        }

        public static void SetWindowButtons (Window d, WindowButton value)
        {
            d.SetValue(WindowButtonsProperty, value);
        }

        private static void WindowButtons_Changed (DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            WindowButtons_Changed((Window)d, new DpChangedEventArgs<WindowButton>(args));
        }

        public static DialogButton GetDialogButtons (Window d)
        {
            return (DialogButton)d.GetValue(DialogButtonsProperty);
        }

        public static void SetDialogButtons (Window d, DialogButton value)
        {
            d.SetValue(DialogButtonsProperty, value);
        }

        public static DialogButton GetButtonResult (ButtonBase d)
        {
            return (DialogButton)d.GetValue(ButtonResultProperty);
        }

        public static void SetButtonResult (ButtonBase d, DialogButton value)
        {
            d.SetValue(ButtonResultProperty, value);
        }

        private static void ButtonResult_Changed (DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            ButtonResult_Changed((ButtonBase)d, new DpChangedEventArgs<DialogButton>(args));
        }

        public static DialogButton GetDialogResult (Window d)
        {
            return (DialogButton)d.GetValue(DialogResultProperty);
        }

        public static void SetDialogResult (Window d, DialogButton value)
        {
            d.SetValue(DialogResultProperty, value);
        }

    }
}
