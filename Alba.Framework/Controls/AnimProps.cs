using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media.Animation;
using Alba.Framework.Events;
using Alba.Framework.Text;

// ReSharper disable UnusedParameter.Local
namespace Alba.Framework.Controls
{
    public static partial class AnimProps
    {
        private static void Target_Changed (Timeline timeline, DpChangedEventArgs<string> args)
        {
            string[] nameAndProp = args.NewValue.Split(new[] { '.' }, 2);
            if (nameAndProp.Length != 2)
                throw new ArgumentException("Target must be a name and a property, separated by a dot.");
            if (!nameAndProp[0].IsNullOrEmpty())
                Storyboard.SetTargetName(timeline, nameAndProp[0]);
            if (!nameAndProp[1].IsNullOrEmpty())
                Storyboard.SetTargetProperty(timeline, new PropertyPath(nameAndProp[1]));
        }

        private static void Animate_Changed (FrameworkElement ctl, DpChangedEventArgs<bool> args)
        {
            SetAnimations(ctl, new AnimationCollection(ctl));
        }
    }

    public class AnimationCollection : Collection<Animation>
    {
        private readonly FrameworkElement _control;
        private readonly Style _style;

        public AnimationCollection (FrameworkElement control)
        {
            _control = control;
            _style = new Style(control.GetType(), control.Style);
        }

        protected override void InsertItem (int index, Animation anim)
        {
            base.InsertItem(index, anim);
            var trigger = new DataTrigger { Binding = anim.Binding, Value = anim.Value };
            var action = new BeginStoryboard { Storyboard = new Storyboard { Children = anim.Children } };
            if (anim.Dir == AnimationDir.Enter)
                trigger.EnterActions.Add(action);
            else
                trigger.ExitActions.Add(action);
            _style.Triggers.Add(trigger);
            if (anim.Last)
                _control.Style = _style;
        }
    }

    [ContentProperty ("Children")]
    public class Animation
    {
        public Binding Binding { get; set; }
        public object Value { get; set; }
        public AnimationDir Dir { get; set; }
        public bool Last { get; set; }
        public TimelineCollection Children { get; private set; }

        public Animation ()
        {
            Children = new TimelineCollection();
        }
    }

    public enum AnimationDir
    {
        Enter,
        Exit,
    }
}