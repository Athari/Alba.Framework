// ReSharper disable RedundantUsingDirective
// ReSharper disable RedundantCast
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using Alba.Framework.Events;
using FPM = System.Windows.FrameworkPropertyMetadata;
using FPMO = System.Windows.FrameworkPropertyMetadataOptions;

namespace Alba.Framework.Controls
{
    public static partial class RangeProps
    {
        public static readonly DependencyProperty MinIntProperty = DependencyProperty.RegisterAttached(
            "MinInt", typeof(int), typeof(RangeProps),
            new FPM(default(int), FPMO.BindsTwoWayByDefault, MinInt_Changed));

        public static readonly DependencyProperty MaxIntProperty = DependencyProperty.RegisterAttached(
            "MaxInt", typeof(int), typeof(RangeProps),
            new FPM(default(int), FPMO.BindsTwoWayByDefault, MaxInt_Changed));

        public static int GetMinInt (RangeBase d)
        {
            return (int)d.GetValue(MinIntProperty);
        }

        public static void SetMinInt (RangeBase d, int value)
        {
            d.SetValue(MinIntProperty, value);
        }

        private static void MinInt_Changed (DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            MinInt_Changed((RangeBase)d, new DpChangedEventArgs<int>(args));
        }

        public static int GetMaxInt (RangeBase d)
        {
            return (int)d.GetValue(MaxIntProperty);
        }

        public static void SetMaxInt (RangeBase d, int value)
        {
            d.SetValue(MaxIntProperty, value);
        }

        private static void MaxInt_Changed (DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            MaxInt_Changed((RangeBase)d, new DpChangedEventArgs<int>(args));
        }

    }
}
