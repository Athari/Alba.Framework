// ReSharper disable RedundantUsingDirective
// ReSharper disable RedundantCast
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using Alba.Framework.Events;
using System.Windows.Media.Animation;
using FPM = System.Windows.FrameworkPropertyMetadata;
using FPMO = System.Windows.FrameworkPropertyMetadataOptions;

namespace Alba.Framework.Controls
{
    public static partial class AnimProps
    {
        public static readonly DependencyProperty TargetProperty = DependencyProperty.RegisterAttached(
            "Target", typeof(string), typeof(AnimProps),
            new FPM(default(string), Target_Changed));

        public static string GetTarget (Timeline d)
        {
            return (string)d.GetValue(TargetProperty);
        }

        public static void SetTarget (Timeline d, string value)
        {
            d.SetValue(TargetProperty, value);
        }

        private static void Target_Changed (DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            Target_Changed((Timeline)d, new DpChangedEventArgs<string>(args));
        }

    }
}
