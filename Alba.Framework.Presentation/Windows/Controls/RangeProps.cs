using System.Windows.Controls.Primitives;
using Alba.Framework.Events;

namespace Alba.Framework.Windows.Controls
{
    public static partial class RangeProps
    {
        private static void MinInt_Changed (RangeBase slider, DpChangedEventArgs<int> args)
        {
            slider.SetCurrentValue(RangeBase.MinimumProperty, (double)args.NewValue);
        }

        private static void MaxInt_Changed (RangeBase slider, DpChangedEventArgs<int> args)
        {
            slider.SetCurrentValue(RangeBase.MaximumProperty, (double)args.NewValue);
        }
    }
}