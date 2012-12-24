// ReSharper disable RedundantUsingDirective
// ReSharper disable RedundantCast
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Alba.Framework.Events;
using FPM = System.Windows.FrameworkPropertyMetadata;
using FPMO = System.Windows.FrameworkPropertyMetadataOptions;

namespace Alba.Framework.Controls
{
    public static partial class ProgressBarProps
    {
        public static readonly DependencyProperty AnimationVisibilityProperty = DependencyProperty.RegisterAttached(
            "AnimationVisibility", typeof(Visibility), typeof(ProgressBarProps),
            new FPM(default(Visibility), AnimationVisibility_Changed));

        public static readonly DependencyProperty IsErrorProperty = DependencyProperty.RegisterAttached(
            "IsError", typeof(bool), typeof(ProgressBarProps),
            new FPM(default(bool)));

        public static Visibility GetAnimationVisibility (ProgressBar d)
        {
            return (Visibility)d.GetValue(AnimationVisibilityProperty);
        }

        public static void SetAnimationVisibility (ProgressBar d, Visibility value)
        {
            d.SetValue(AnimationVisibilityProperty, value);
        }

        private static void AnimationVisibility_Changed (DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            AnimationVisibility_Changed((ProgressBar)d, new DpChangedEventArgs<Visibility>(args));
        }

        public static bool GetIsError (ProgressBar d)
        {
            return (bool)d.GetValue(IsErrorProperty);
        }

        public static void SetIsError (ProgressBar d, bool value)
        {
            d.SetValue(IsErrorProperty, value);
        }

    }
}
