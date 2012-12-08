using System;
using System.Collections.Generic;
using Alba.Framework.Events;
using Alba.Framework.Collections;
using ExecuteEventHandler = System.EventHandler<Alba.Framework.Commands.ExecuteEventArgs>;
using CanExecuteEventHandler = System.EventHandler<Alba.Framework.Commands.CanExecuteEventArgs>;

namespace Alba.Framework.Commands
{
    public static class Commands
    {
        private static readonly CommandsHandlersDictionary _commandsHandlers = new CommandsHandlersDictionary();

        public static void Subscribe (List<object> refs, object model, EventCommand command, Action execute, Func<bool> canExecute = null)
        {
            ModelHandlers handlers = GetModelHandlers(command, model, true);

            ExecuteEventHandler executeHandler = (s, a) => execute();
            WeakEvents.AddHandler(ref handlers.ExecuteHandlers, executeHandler);
            refs.Add(executeHandler);

            if (canExecute != null) {
                CanExecuteEventHandler canExecuteHandler = (s, a) => { a.CanExecute = canExecute(); };
                WeakEvents.AddHandler(ref handlers.CanExecuteHandlers, canExecuteHandler);
                refs.Add(canExecuteHandler);
            }
        }

        public static void Subscribe<T> (List<object> refs, object model, EventCommand command, Action<T> execute, Func<T, bool> canExecute = null)
        {
            ModelHandlers handlers = GetModelHandlers(command, model, true);

            ExecuteEventHandler executeHandler = (s, a) => execute((T)a.Parameter);
            WeakEvents.AddHandler(ref handlers.ExecuteHandlers, executeHandler);
            refs.Add(executeHandler);

            if (canExecute != null) {
                CanExecuteEventHandler canExecuteHandler = (s, a) => { a.CanExecute = canExecute((T)a.Parameter); };
                WeakEvents.AddHandler(ref handlers.CanExecuteHandlers, canExecuteHandler);
                refs.Add(canExecuteHandler);
            }
        }

        private static void CommandExecute (object sender, ExecuteEventArgs args)
        {
            ModelHandlers handlers = GetModelHandlers((EventCommand)sender, args.Model, false);
            if (handlers == null)
                return;
            WeakEvents.Call<ExecuteEventArgs>(handlers.ExecuteHandlers, h => h(sender, args));
        }

        private static void CommandCanExecute (object sender, CanExecuteEventArgs args)
        {
            ModelHandlers handlers = GetModelHandlers((EventCommand)sender, args.Model, false);
            if (handlers == null)
                return;
            WeakEvents.Call<CanExecuteEventArgs>(handlers.CanExecuteHandlers, h => h(sender, args));
        }

        private static ModelHandlers GetModelHandlers (EventCommand command, object model, bool create)
        {
            if (create) {
                ModelsHandlersDictionary modelsHandlers = _commandsHandlers.GetOrAdd(command, () => {
                    command.Subscribe(CommandExecute, CommandCanExecute);
                    return new ModelsHandlersDictionary();
                });
                return modelsHandlers.GetOrAdd(model, () => new ModelHandlers());
            }
            else {
                ModelsHandlersDictionary modelsHandlers = _commandsHandlers.GetOrDefault(command);
                if (modelsHandlers == null)
                    return null;
                return modelsHandlers.GetOrDefault(model);
            }
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