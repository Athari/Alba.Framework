using System;
using System.Windows;
using System.Windows.Media.Animation;
using Alba.Framework.Attributes;

namespace Alba.Framework.Wpf
{
    public static class StoryboardExts
    {
        public static T SetTarget<T> (this T element, DependencyObject target, PropertyPath property)
            where T : Timeline
        {
            Storyboard.SetTarget(element, target);
            Storyboard.SetTargetProperty(element, property);
            return element;
        }

        public static T SetTarget<T> (this T element, string targetName, PropertyPath property)
            where T : Timeline
        {
            Storyboard.SetTargetName(element, targetName);
            Storyboard.SetTargetProperty(element, property);
            return element;
        }

        public static T SetTarget<T> (this T element, string targetName, DependencyProperty dp)
            where T : Timeline
        {
            Storyboard.SetTargetName(element, targetName);
            Storyboard.SetTargetProperty(element, dp.ToPath());
            return element;
        }

        public static T SetTarget<T> (this T element, string targetName, string path)
            where T : Timeline
        {
            Storyboard.SetTargetName(element, targetName);
            Storyboard.SetTargetProperty(element, new PropertyPath(path));
            return element;
        }

        public static T SetTarget<T> (this T element, PropertyPath property)
            where T : Timeline
        {
            Storyboard.SetTargetProperty(element, property);
            return element;
        }

        public static T SetTarget<T> (this T element, DependencyProperty dp)
            where T : Timeline
        {
            Storyboard.SetTargetProperty(element, dp.ToPath());
            return element;
        }

        public static T SetTarget<T> (this T element, string path)
            where T : Timeline
        {
            Storyboard.SetTargetProperty(element, new PropertyPath(path));
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

        public static Storyboard AnimateDouble (this Storyboard @this, TimeSpan duration, double? from = null, double? to = null, double? by = null,
            double accel = 0, double decel = 0,
            [InstantHandle] Action<DoubleAnimation> a = null)
        {
            var anim = new DoubleAnimation {
                Duration = new Duration(duration), From = @from, To = to, By = null,
                AccelerationRatio = accel, DecelerationRatio = decel,
            };
            if (a != null)
                a(anim);
            @this.Children.Add(anim);
            return @this;
        }

        public static DoubleAnimationUsingPath CloneForY (this DoubleAnimationUsingPath animX)
        {
            var animY = animX.Clone();
            animY.Source = PathAnimationSource.Y;
            return animY;
        }
    }
}