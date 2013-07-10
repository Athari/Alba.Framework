// ReSharper disable RedundantUsingDirective
// ReSharper disable RedundantCast
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using Alba.Framework.Interop;
using FPM = System.Windows.FrameworkPropertyMetadata;
using FPMO = System.Windows.FrameworkPropertyMetadataOptions;

namespace Alba.Framework.Windows.Controls
{
    public static partial class WindowProps
    {
        public static readonly DependencyProperty PlacementProperty = DependencyProperty.RegisterAttached(
            "Placement", typeof(WINDOWPLACEMENT), typeof(WindowProps),
            new FPM((WINDOWPLACEMENT)WINDOWPLACEMENT.Invalid, FPMO.BindsTwoWayByDefault, Placement_Changed));

        private static readonly DependencyProperty NormalPositionProperty = DependencyProperty.RegisterAttached(
            "NormalPosition", typeof(Rect), typeof(WindowProps),
            new FPM(default(Rect)));

        public static WINDOWPLACEMENT GetPlacement (Window d)
        {
            return (WINDOWPLACEMENT)d.GetValue(PlacementProperty);
        }

        public static void SetPlacement (Window d, WINDOWPLACEMENT value)
        {
            d.SetValue(PlacementProperty, value);
        }

        private static void Placement_Changed (DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            Placement_Changed((Window)d, new DpChangedEventArgs<WINDOWPLACEMENT>(args));
        }

        private static Rect GetNormalPosition (Window d)
        {
            return (Rect)d.GetValue(NormalPositionProperty);
        }

        private static void SetNormalPosition (Window d, Rect value)
        {
            d.SetValue(NormalPositionProperty, value);
        }

    }
}
