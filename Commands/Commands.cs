using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Windows;
using System.Windows.Input;
using Alba.Framework.Events;
using Alba.Framework.Collections;
using Alba.Framework.Linq;
using DpChangedEventArgs = System.Windows.DependencyPropertyChangedEventArgs;

namespace Alba.Framework.Commands
{
    public static class Commands
    {
        private static readonly CommandsHandlersDictionary _commandsHandlers = new CommandsHandlersDictionary();
        private static readonly KeyGestureConverter _keyGestureConverter = new KeyGestureConverter();

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command", typeof(EventCommandRef), typeof(Commands),
                new PropertyMetadata(CommandProperty_Changed));
        public static readonly DependencyProperty CommandBindingsProperty =
            DependencyProperty.RegisterAttached("CommandBindings", typeof(IEnumerable<CommandBinding>), typeof(Commands),
                new PropertyMetadata(CommandBindingsProperty_Changed));

        public static void SetCommand (DependencyObject d, EventCommandRef value)
        {
            d.SetValue(CommandProperty, value);
        }

        public static EventCommandRef GetCommand (DependencyObject d)
        {
            return (EventCommandRef)d.GetValue(CommandProperty);
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
            Properties.SetDpValue(d, GetName(o => o.Command), ((EventCommandRef)e.NewValue).Command);
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

        public static void Subscribe (List<object> refs, object model, EventCommand command, Action execute, Func<bool> canExecute = null)
        {
            ModelsHandlersDictionary modelsHandlers = _commandsHandlers.GetOrAdd(command, () => {
                command.Subscribe(CommandExecute, CommandCanExecute);
                return new ModelsHandlersDictionary();
            });
            ModelHandlers handlers = modelsHandlers.GetOrAdd(model, () => new ModelHandlers());

            EventHandler<ExecuteEventArgs> executeHandler = (s, a) => execute();
            WeakEvents.AddHandler(ref handlers.ExecuteHandlers, executeHandler);
            refs.Add(executeHandler);

            if (canExecute != null) {
                EventHandler<CanExecuteEventArgs> canExecuteHandler = (s, a) => { a.CanExecute = canExecute(); };
                WeakEvents.AddHandler<CanExecuteEventArgs>(ref handlers.CanExecuteHandlers, canExecuteHandler);
                refs.Add(canExecuteHandler);
            }
        }

        private static void CommandExecute (object sender, ExecuteEventArgs args)
        {
            ModelHandlers handlers = GetModelHandlers((EventCommand)sender, args.Model);
            if (handlers == null)
                return;
            WeakEvents.Call<ExecuteEventArgs>(handlers.ExecuteHandlers, h => h(sender, args));
        }

        private static void CommandCanExecute (object sender, CanExecuteEventArgs args)
        {
            ModelHandlers handlers = GetModelHandlers((EventCommand)sender, args.Model);
            if (handlers == null)
                return;
            WeakEvents.Call<CanExecuteEventArgs>(handlers.CanExecuteHandlers, h => h(sender, args));
        }

        private static ModelHandlers GetModelHandlers (EventCommand command, object model)
        {
            ModelsHandlersDictionary modelsHandlers = _commandsHandlers.GetOrDefault(command);
            if (modelsHandlers == null)
                return null;
            return modelsHandlers.GetOrDefault(model);
        }

        private static string GetName<TProp> (Expression<Func<ICommandSource, TProp>> expr)
        {
            return Properties.GetName(expr);
        }

        private class CommandsHandlersDictionary : Dictionary<EventCommand, ModelsHandlersDictionary>
        {}

        private class ModelsHandlersDictionary : Dictionary<object, ModelHandlers>
        {}

        private class ModelHandlers
        {
            public List<WeakReference> ExecuteHandlers;
            public List<WeakReference> CanExecuteHandlers;
        }
    }
}