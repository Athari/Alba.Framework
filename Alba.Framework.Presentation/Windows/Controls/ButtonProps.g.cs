// ReSharper disable RedundantUsingDirective
// ReSharper disable RedundantCast
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using FPM = System.Windows.FrameworkPropertyMetadata;
using FPMO = System.Windows.FrameworkPropertyMetadataOptions;

namespace Alba.Framework.Windows.Controls
{
    public static partial class ButtonProps
    {
        public static readonly DependencyProperty IconProperty = DependencyProperty.RegisterAttached(
            "Icon", typeof(ImageSource), typeof(ButtonProps),
            new FPM(default(ImageSource)));

        public static ImageSource GetIcon (Button d)
        {
            return (ImageSource)d.GetValue(IconProperty);
        }

        public static void SetIcon (Button d, ImageSource value)
        {
            d.SetValue(IconProperty, value);
        }

    }
}
