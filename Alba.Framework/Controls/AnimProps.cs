using System;
using System.Windows;
using System.Windows.Media.Animation;
using Alba.Framework.Events;

namespace Alba.Framework.Controls
{
    public static partial class AnimProps
    {
        private static void Target_Changed (Timeline timeline, DpChangedEventArgs<string> args)
        {
            string[] nameAndProp = args.NewValue.Split(new[] { '.' }, 2);
            if (nameAndProp.Length != 2)
                throw new ArgumentException("Target must be a name and a property, separated by a dot.");
            Storyboard.SetTargetName(timeline, nameAndProp[0]);
            Storyboard.SetTargetProperty(timeline, new PropertyPath(nameAndProp[1]));
        }
    }
}