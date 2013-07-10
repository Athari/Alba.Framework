using System.Windows;
using System.Windows.Controls;
using Alba.Framework.Events;

namespace Alba.Framework.Windows.Controls
{
    public static partial class ProgressBarProps
    {
        private static void AnimationVisibility_Changed (ProgressBar progress, DpChangedEventArgs<Visibility> args)
        {
            RoutedEventHandler handler = null;
            handler = (s, a) => {
                var animation = progress.Template.FindName("Animation", progress) as UIElement;
                if (animation != null)
                    animation.Visibility = args.NewValue;
                progress.Loaded -= handler;
            };
            if (progress.Template == null)
                progress.Loaded += handler;
            else
                handler(progress, null);
        }
    }
}