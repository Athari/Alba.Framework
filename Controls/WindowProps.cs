using System;
using System.Windows;
using Alba.Framework.Events;
using Alba.Framework.Interop;

namespace Alba.Framework.Controls
{
    public static partial class WindowProps
    {
        private static void Placement_Changed (Window window, DpChangedEventArgs<WINDOWPLACEMENT> args)
        {
            if (!args.NewValue.IsEmpty)
                window.InvokeOnHandle(() => window.SetWindowPlacement(args.NewValue));
            EnsureWindowPlaceSubscribtions(window);
        }

        private static void EnsureWindowPlaceSubscribtions (Window window)
        {
            window.InvokeOnHandle(() => {
                window.LocationChanged -= Window_PlacementChanged;
                window.LocationChanged += Window_PlacementChanged;
                window.SizeChanged -= Window_PlacementChanged;
                window.SizeChanged += Window_PlacementChanged;
                window.StateChanged -= Window_PlacementChanged;
                window.StateChanged += Window_PlacementChanged;
            });
        }

        private static void Window_PlacementChanged (object sender, EventArgs args)
        {
            var window = (Window)sender;
            window.SetCurrentValue(PlacementProperty, window.GetWindowPlacement());
        }
    }
}