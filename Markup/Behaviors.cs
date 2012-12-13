using System;
using System.Linq.Expressions;
using System.Windows;
using Alba.Framework.Interop;
using Alba.Framework.Linq;
using DpChangedEventArgs = System.Windows.DependencyPropertyChangedEventArgs;
using Alba.Framework.System;

namespace Alba.Framework.Markup
{
    public static class Behaviors
    {
        public static DependencyProperty WindowButtonsProperty = DependencyProperty.RegisterAttached(
            GetName(() => WindowButtonsProperty), typeof(WindowButtons), typeof(Behaviors),
            new PropertyMetadata(WindowButtons.Default, WindowButtons_Changed));

        public static WindowButtons GetWindowButtons (Window d)
        {
            return (WindowButtons)d.GetValue(WindowButtonsProperty);
        }

        public static void SetWindowButtons (Window d, WindowButtons value)
        {
            d.SetValue(WindowButtonsProperty, value);
        }

        private static void WindowButtons_Changed (DependencyObject d, DpChangedEventArgs args)
        {
            var window = (Window)d;
            var value = (WindowButtons)args.NewValue;
            EventHandler changeButtons = (s, a) => {
                WS style = window.GetWindowStyle();
                WS_EX styleEx = window.GetWindowStyleEx();
                SetBitIf(ref style, WS.SYSMENU, value & WindowButtons.System);
                SetBitIf(ref style, WS.MINIMIZEBOX, value & WindowButtons.Minimize);
                SetBitIf(ref style, WS.MAXIMIZEBOX, value & WindowButtons.Maximize);
                SetBitIf(ref styleEx, WS_EX.CONTEXTHELP, value & WindowButtons.Help);
                SetBitIf(ref styleEx, WS_EX.DLGMODALFRAME, ~(value & WindowButtons.Icon));
                window.SetWindowStyle(style);
                window.SetWindowStyleEx(styleEx);
                window.SetWindowPos(flags: SWP.FRAMECHANGED);
            };
            if (window.IsHandleInitialized())
                changeButtons(null, EventArgs.Empty);
            else
                window.SourceInitialized += changeButtons;
        }

        private static void SetBitIf<T, U> (ref T style, U bit, WindowButtons value)
        {
            var ustyle = style.To<uint>();
            var ubit = bit.To<uint>();
            style = (value != 0 ? ustyle | ubit : ustyle & ~ubit).To<T>();
        }

        private static string GetName (Expression<Func<DependencyProperty>> propExpr)
        {
            return Properties.GetName(propExpr);
        }
    }
}