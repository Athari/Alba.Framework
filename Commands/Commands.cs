using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Alba.Framework.Events;
using Alba.Framework.Collections;
using Alba.Framework.Linq;
using Alba.Framework.System;
using ExecuteEventHandler = System.EventHandler<Alba.Framework.Commands.ExecuteEventArgs>;
using CanExecuteEventHandler = System.EventHandler<Alba.Framework.Commands.CanExecuteEventArgs>;

namespace Alba.Framework.Commands
{
    public static class Commands
    {
        private const string ErrorUnexpectedCommandParam = "Command '{0}' expected parameter of type '{1}', but received '{2}'.";

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

            ExecuteEventHandler executeHandler = InvokeWithParamTypeCheck<ExecuteEventArgs, T>(command,
                a => execute((T)a.Parameter));
            WeakEvents.AddHandler(ref handlers.ExecuteHandlers, executeHandler);
            refs.Add(executeHandler);

            if (canExecute != null) {
                CanExecuteEventHandler canExecuteHandler = InvokeWithParamTypeCheck<CanExecuteEventArgs, T>(command,
                    a => a.CanExecute = canExecute((T)a.Parameter));
                WeakEvents.AddHandler(ref handlers.CanExecuteHandlers, canExecuteHandler);
                refs.Add(canExecuteHandler);
            }
        }

        private static EventHandler<TArgs> InvokeWithParamTypeCheck<TArgs, TParam> (
            EventCommand command, Action<TArgs> handler)
            where TArgs : ExecuteEventArgs
        {
            return (s, a) => {
                if (a.Parameter is TParam)
                    handler(a);
                else
                    throw new ArgumentException(string.Format(ErrorUnexpectedCommandParam,
                        command.Name, typeof(TParam).FullName, a.Parameter.GetTypeFullName()));
            };
        }

        private static void OnCommandExecute (object sender, ExecuteEventArgs args)
        {
            ModelHandlers handlers = GetModelHandlers((EventCommand)sender, args.Model, false);
            if (handlers == null)
                return;
            WeakEvents.Call<ExecuteEventArgs>(handlers.ExecuteHandlers, h => h(sender, args));
        }

        private static void OnCommandCanExecute (object sender, CanExecuteEventArgs args)
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
                    command.Subscribe(OnCommandExecute, OnCommandCanExecute);
                    return new ModelsHandlersDictionary();
                });
                return modelsHandlers.GetOrAdd(model, () => new ModelHandlers());
            }
            else {
                ModelsHandlersDictionary modelsHandlers = _commandsHandlers.GetOrDefault(command);
                return modelsHandlers == null ? null : modelsHandlers.GetOrDefault(model);
            }
        }

        public static EventCommand Register (Expression<Func<EventCommand>> propExpr, bool isAutoRequery = true)
        {
            return new EventCommand(Properties.GetName(propExpr), isAutoRequery);
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