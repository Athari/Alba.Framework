// ReSharper disable RedundantUsingDirective
// ReSharper disable RedundantCast
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Animation;
using FPM = System.Windows.FrameworkPropertyMetadata;
using FPMO = System.Windows.FrameworkPropertyMetadataOptions;

namespace Alba.Framework.Windows.Controls
{
    public static partial class AnimProps
    {
        public static readonly DependencyProperty TargetProperty = DependencyProperty.RegisterAttached(
            "Target", typeof(string), typeof(AnimProps),
            new FPM(default(string), Target_Changed));

        public static readonly DependencyProperty AnimateProperty = DependencyProperty.RegisterAttached(
            "Animate", typeof(bool), typeof(AnimProps),
            new FPM(default(bool), Animate_Changed));

        public static readonly DependencyProperty AnimationsProperty = DependencyProperty.RegisterAttached(
            "Animations", typeof(AnimationCollection), typeof(AnimProps),
            new FPM(default(AnimationCollection)));

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

        public static bool GetAnimate (FrameworkElement d)
        {
            return (bool)d.GetValue(AnimateProperty);
        }

        public static void SetAnimate (FrameworkElement d, bool value)
        {
            d.SetValue(AnimateProperty, value);
        }

        private static void Animate_Changed (DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            Animate_Changed((FrameworkElement)d, new DpChangedEventArgs<bool>(args));
        }

        public static AnimationCollection GetAnimations (FrameworkElement d)
        {
            return (AnimationCollection)d.GetValue(AnimationsProperty);
        }

        public static void SetAnimations (FrameworkElement d, AnimationCollection value)
        {
            d.SetValue(AnimationsProperty, value);
        }

    }
}
