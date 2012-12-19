using System.Windows;
using Alba.Framework.System;

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

    }
}
