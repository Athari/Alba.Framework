using System.Windows;

// ReSharper disable RedundantCast
namespace Alba.Framework.Markup
{
    public static partial class Behaviors
    {
        public static DependencyProperty WindowButtonsProperty = DependencyProperty.RegisterAttached(
            "WindowButtons", typeof(WindowButton), typeof(Behaviors),
            new PropertyMetadata((WindowButton)WindowButton.Default, WindowButtons_Changed));
        public static DependencyProperty DialogButtonsProperty = DependencyProperty.RegisterAttached(
            "DialogButtons", typeof(DialogButton), typeof(Behaviors),
            new PropertyMetadata(default(DialogButton)));

        public static WindowButton GetWindowButtons (Window d)
        {
            return (WindowButton)d.GetValue(WindowButtonsProperty);
        }

        public static void SetWindowButtons (Window d, WindowButton value)
        {
            d.SetValue(WindowButtonsProperty, value);
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
