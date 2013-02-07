using System;
using System.Windows;
using Alba.Framework.Common;
using Alba.Framework.Events;
using Alba.Framework.Interop;
using Alba.Framework.Sys;

namespace Alba.Framework.Controls
{
    public static partial class WindowProps
    {
        [ThreadStatic]
        private static bool _isChangingPlace;

        private static void Placement_Changed (Window window, DpChangedEventArgs<WINDOWPLACEMENT> args)
        {
            if (_isChangingPlace)
                return;
            WINDOWPLACEMENT place = args.NewValue;
            if (!args.NewValue.IsEmpty) {
                window.WindowStartupLocation = WindowStartupLocation.Manual;
                window.Left = place.NormalPosition.Left;
                window.Top = place.NormalPosition.Top;
                window.Width = place.NormalPosition.Width;
                window.Height = place.NormalPosition.Height;
                SetNormalPosition(window, place.NormalPosition);
                window.WindowState = place.ShowCmd == SW.MAXIMIZE ? WindowState.Maximized : WindowState.Normal;
            }
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
            using (new BoolMonitor(ref _isChangingPlace, () => _isChangingPlace = false)) {
                var window = (Window)sender;
                // Windows when maximizing first moves window to (-8;-8), then resizes to max
                if (window.WindowState == WindowState.Normal && (!window.Left.IsCloseTo(-8) || !window.Top.IsCloseTo(-8)))
                    SetNormalPosition(window, new Rect(window.Left, window.Top, window.Width, window.Height));
                window.SetCurrentValue(PlacementProperty, new WINDOWPLACEMENT {
                    NormalPosition = GetNormalPosition(window),
                    ShowCmd = window.WindowState == WindowState.Maximized ? SW.MAXIMIZE : SW.SHOWNORMAL,
                });
            }
        }
    }
}