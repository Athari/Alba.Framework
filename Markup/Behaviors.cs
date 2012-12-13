using System;
using System.Linq.Expressions;
using System.Windows;
using Alba.Framework.Interop;
using Alba.Framework.Linq;
using Alba.Framework.System;
using DpChangedEventArgs = System.Windows.DependencyPropertyChangedEventArgs;

namespace Alba.Framework.Markup
{
    public static class Behaviors
    {
        public static DependencyProperty WindowButtonsProperty = RegisterAttachedProperty<WindowButton>(
            () => WindowButtonsProperty, new PropertyMetadata(WindowButton.Default, WindowButtons_Changed));
        public static DependencyProperty DialogButtonsProperty = RegisterAttachedProperty<DialogButton>(
            () => DialogButtonsProperty, new PropertyMetadata());

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

        private static void WindowButtons_Changed (DependencyObject d, DpChangedEventArgs args)
        {
            var window = (Window)d;
            var value = (WindowButton)args.NewValue;
            EventHandler changeButtons = (s, a) => {
                WS style = window.GetWindowStyle();
                WS_EX styleEx = window.GetWindowStyleEx();
                SetBitIf(ref style, WS.SYSMENU, value & WindowButton.System);
                SetBitIf(ref style, WS.MINIMIZEBOX, value & WindowButton.Minimize);
                SetBitIf(ref style, WS.MAXIMIZEBOX, value & WindowButton.Maximize);
                SetBitIf(ref styleEx, WS_EX.CONTEXTHELP, value & WindowButton.Help);
                SetBitIf(ref styleEx, WS_EX.DLGMODALFRAME, ~(value & WindowButton.Icon));
                window.SetWindowStyle(style);
                window.SetWindowStyleEx(styleEx);
                window.SetWindowPos(flags: SWP.FRAMECHANGED);
            };
            if (window.IsHandleInitialized())
                changeButtons(null, EventArgs.Empty);
            else
                window.SourceInitialized += changeButtons;
        }

        private static void SetBitIf<T, U> (ref T style, U bit, WindowButton value)
        {
            var ustyle = style.To<uint>();
            var ubit = bit.To<uint>();
            style = (value != 0 ? ustyle | ubit : ustyle & ~ubit).To<T>();
        }

        private static DependencyProperty RegisterAttachedProperty<T> (Expression<Func<DependencyProperty>> propExpr,
            PropertyMetadata metadata)
        {
            return DependencyProperty.RegisterAttached(Properties.GetName(propExpr).RemovePostfix("Property"),
                typeof(T), typeof(Behaviors), metadata);
        }
    }
}