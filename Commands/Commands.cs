using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace Alba.Framework.Commands
{
    public static class Commands
    {
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command", typeof(CommandDisplay), typeof(Commands),
                new PropertyMetadata(CommandProperty_Changed));

        public static void SetCommand (DependencyObject d, CommandDisplay value)
        {
            d.SetValue(CommandProperty, value);
        }

        public static CommandDisplay GetCommand (DependencyObject d)
        {
            return (CommandDisplay)d.GetValue(CommandProperty);
        }

        private static void CommandProperty_Changed (DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Type type = d.GetType();
            RoutedCommand command = ((CommandDisplay)e.NewValue).Command;
            DependencyPropertyDescriptor.FromName("Command", type, type).SetValue(d, command);
        }
    }
}