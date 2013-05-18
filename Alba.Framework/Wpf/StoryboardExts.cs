using System;
using System.Windows;
using System.Windows.Media.Animation;

namespace Alba.Framework.Wpf
{
    public static class StoryboardExts
    {
        public static T SetTarget<T> (this T element, PropertyPath value)
            where T : Timeline
        {
            Storyboard.SetTargetProperty(element, value);
            return element;
        }

        public static T SetBeginTime<T> (this T element, TimeSpan value)
            where T : Timeline
        {
            element.BeginTime = value;
            return element;
        }

        public static T AddCompleted<T> (this T element, EventHandler value)
            where T : Timeline
        {
            EventHandler handler = null;
            handler = (s, a) => {
                value(s, a);
                element.Completed -= handler;
            };
            element.Completed += handler;
            return element;
        }

        public static Storyboard AnimateDouble (this Storyboard @this, TimeSpan duration, double? from = null, double? to = null, double? by = null, Action<DoubleAnimation> a = null)
        {
            var anim = new DoubleAnimation { Duration = new Duration(duration), From = @from, To = to, By = null };
            if (a != null)
                a(anim);
            @this.Children.Add(anim);
            return @this;
        }
    }
}