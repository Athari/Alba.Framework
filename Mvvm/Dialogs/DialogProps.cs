using System;
using System.Diagnostics.Contracts;
using System.Windows;
using System.Windows.Controls.Primitives;
using Alba.Framework.Events;
using Alba.Framework.Interop;
using Alba.Framework.System;

namespace Alba.Framework.Mvvm.Dialogs
{
    public static partial class DialogProps
    {
        private static void WindowButtons_Changed (Window window, DpChangedEventArgs<WindowButton> args)
        {
            EventHandler changeButtons = (s, a) => {
                WS style = window.GetWindowStyle();
                WS_EX styleEx = window.GetWindowStyleEx();
                SetBitIf(ref style, WS.SYSMENU, args.NewValue & WindowButton.System);
                SetBitIf(ref style, WS.MINIMIZEBOX, args.NewValue & WindowButton.Minimize);
                SetBitIf(ref style, WS.MAXIMIZEBOX, args.NewValue & WindowButton.Maximize);
                SetBitIf(ref styleEx, WS_EX.CONTEXTHELP, args.NewValue & WindowButton.Help);
                SetBitIf(ref styleEx, WS_EX.DLGMODALFRAME, ~(args.NewValue & WindowButton.Icon));
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

        private static void ButtonResult_Changed (ButtonBase button, DpChangedEventArgs<DialogButton> args)
        {
            button.Click += (s, a) => {
                var win = Window.GetWindow(button);
                Contract.Assume(win != null);
                SetDialogResult(win, args.NewValue);
                win.Close();
            };
        }
    }
}