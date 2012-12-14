using System;
using System.Windows;
using Alba.Framework.Interop;
using Alba.Framework.System;
using DpChangedEventArgs = System.Windows.DependencyPropertyChangedEventArgs;

namespace Alba.Framework.Markup
{
    public static partial class Behaviors
    {
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
    }
}