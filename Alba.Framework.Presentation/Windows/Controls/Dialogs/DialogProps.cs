using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using Alba.Framework.Sys;
using Alba.Framework.Windows.Interop;

namespace Alba.Framework.Windows.Controls
{
    public static partial class DialogProps
    {
        private static void WindowButtons_Changed (Window window, DpChangedEventArgs<WindowButton> args)
        {
            window.InvokeOnHandle(() => {
                WS style = window.GetWindowStyle();
                WS_EX styleEx = window.GetWindowStyleEx();
                SetBitIf(ref style, WS.SYSMENU, args.NewValue & WindowButton.System);
                SetBitIf(ref style, WS.MINIMIZEBOX, args.NewValue & WindowButton.Minimize);
                SetBitIf(ref style, WS.MAXIMIZEBOX, args.NewValue & WindowButton.Maximize);
                SetBitIf(ref styleEx, WS_EX.CONTEXTHELP, args.NewValue & WindowButton.Help);
                SetBitIf(ref styleEx, WS_EX.DLGMODALFRAME, ~(args.NewValue & WindowButton.Icon));
                window.SetWindowStyle(style);
                window.SetWindowStyleEx(styleEx);
                if ((args.NewValue & WindowButton.Icon) == 0)
                    window.ClearWindowIcons();
                window.SetWindowPos(flags: SWP.FRAMECHANGED);
            });
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

        private static void DialogButtons_Changed (Window window, DpChangedEventArgs<DialogButton> args)
        {
            if (args != null && GetLeftDialogButtons(window) == null) // args check to get rid of unused warning
                SetLeftDialogButtons(window, new ObservableCollection<Visual>());
        }
    }
}