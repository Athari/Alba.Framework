using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Windows;
using System.Windows.Input;
using Alba.Framework.Linq;
using DpChangedEventArgs = System.Windows.DependencyPropertyChangedEventArgs;

namespace Alba.Framework.Commands
{
    public static class Commands
    {
        private static readonly KeyGestureConverter _keyGestureConverter = new KeyGestureConverter();

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command", typeof(CommandDisplay), typeof(Commands),
                new PropertyMetadata(CommandProperty_Changed));
        public static readonly DependencyProperty CommandBindingsProperty =
            DependencyProperty.RegisterAttached("CommandBindings", typeof(IEnumerable<CommandBinding>), typeof(Commands),
                new PropertyMetadata(CommandBindingsProperty_Changed));

        public static void SetCommand (DependencyObject d, CommandDisplay value)
        {
            d.SetValue(CommandProperty, value);
        }

        public static CommandDisplay GetCommand (DependencyObject d)
        {
            return (CommandDisplay)d.GetValue(CommandProperty);
        }

        public static void SetCommandBindings (DependencyObject d, IEnumerable<CommandBinding> value)
        {
            d.SetValue(CommandBindingsProperty, value);
        }

        public static IEnumerable<CommandBinding> GetCommandBindings (DependencyObject d)
        {
            return (IEnumerable<CommandBinding>)d.GetValue(CommandBindingsProperty);
        }

        private static void CommandProperty_Changed (DependencyObject d, DpChangedEventArgs e)
        {
            Properties.SetDpValue(d, "Command", ((CommandDisplay)e.NewValue).Command);
        }

        private static void CommandBindingsProperty_Changed (DependencyObject d, DpChangedEventArgs e)
        {
            var bindings = Properties.GetValue<CommandBindingCollection>(d, "CommandBindings");
            foreach (var binding in (IEnumerable<CommandBinding>)e.NewValue)
                bindings.Add(binding);
        }

        public static ControlCommand RegisterCommand (Type ownerType, Expression<Func<ControlCommand>> propExpr, params string[] gestures)
        {
            var cmd = new ControlCommand(Properties.GetName(propExpr), ownerType);
            // ReSharper disable AssignNullToNotNullAttribute (converter throws if can't convert)
            foreach (string gesture in gestures)
                cmd.InputGestures.Add((InputGesture)_keyGestureConverter.ConvertFrom(gesture));
            // ReSharper restore AssignNullToNotNullAttribute
            return cmd;
        }
    }
}