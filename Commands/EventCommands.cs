using System;
using System.Linq.Expressions;
using Alba.Framework.Collections;
using Alba.Framework.Events;
using Alba.Framework.Linq;
using Alba.Framework.Mvvm.Models;
using Alba.Framework.System;
using ExecuteEventHandler = System.EventHandler<Alba.Framework.Commands.ExecuteEventArgs>;
using CanExecuteEventHandler = System.EventHandler<Alba.Framework.Commands.CanExecuteEventArgs>;
using ModelsHandlersDictionary = System.Runtime.CompilerServices.ConditionalWeakTable<
    Alba.Framework.Mvvm.Models.IModel,
    Alba.Framework.Commands.EventCommands.ModelHandlers>;
using CommandsHandlersDictionary = System.Collections.Generic.Dictionary<
    Alba.Framework.Commands.EventCommand,
    /*ModelsHandlersDictionary*/ System.Runtime.CompilerServices.ConditionalWeakTable<
        Alba.Framework.Mvvm.Models.IModel,
        Alba.Framework.Commands.EventCommands.ModelHandlers>>;

namespace Alba.Framework.Commands
{
    public static class EventCommands
    {
        private const string ErrorUnexpectedCommandParam = "Command '{0}' expected parameter of type '{1}', but received '{2}'.";

        private static readonly CommandsHandlersDictionary _commandsHandlers = new CommandsHandlersDictionary();

        public static void Subscribe (IModel model, EventCommand command, Action execute, Func<bool> canExecute = null)
        {
            ModelHandlers handlers = GetModelHandlers(command, model, true);

            lock (handlers) {
                handlers.Execute += (s, a) => execute();
                if (canExecute != null)
                    handlers.CanExecute += (s, a) => { a.CanExecute = canExecute(); };
            }
        }

        public static void Subscribe<T> (IModel model, EventCommand command, Action<T> execute, Func<T, bool> canExecute = null)
        {
            ModelHandlers handlers = GetModelHandlers(command, model, true);

            lock (handlers) {
                handlers.Execute += InvokeWithParamTypeCheck<ExecuteEventArgs, T>(command,
                    a => execute((T)a.Parameter));
                if (canExecute != null)
                    handlers.CanExecute += InvokeWithParamTypeCheck<CanExecuteEventArgs, T>(command,
                        a => a.CanExecute = canExecute((T)a.Parameter));
            }
        }

        public static void Unsubscribe (IModel model, EventCommand command)
        {
            lock (_commandsHandlers) {
                ModelsHandlersDictionary modelsHandlers = _commandsHandlers.GetOrDefault(command);
                if (modelsHandlers != null)
                    modelsHandlers.Remove(model);
            }
        }

        public static void Unsubscribe (IModel model)
        {
            lock (_commandsHandlers) {
                foreach (ModelsHandlersDictionary modelsHandlers in _commandsHandlers.Values)
                    modelsHandlers.Remove(model);
            }
        }

        private static EventHandler<TArgs> InvokeWithParamTypeCheck<TArgs, TParam> (
            EventCommand command, Action<TArgs> handler)
            where TArgs : ExecuteEventArgs
        {
            return (s, a) => {
                if (!(a.Parameter is TParam))
                    throw new ArgumentException(string.Format(ErrorUnexpectedCommandParam,
                        command.Name, typeof(TParam).FullName, a.Parameter.GetTypeFullName()));
                handler(a);
            };
        }

        private static void OnCommandExecute (object sender, ExecuteEventArgs args)
        {
            ModelHandlers handlers = GetModelHandlers((EventCommand)sender, args.Model, false);
            if (handlers == null)
                return;
            handlers.RaiseExecute(sender, args);
        }

        private static void OnCommandCanExecute (object sender, CanExecuteEventArgs args)
        {
            ModelHandlers handlers = GetModelHandlers((EventCommand)sender, args.Model, false);
            if (handlers == null)
                return;
            handlers.RaiseCanExecute(sender, args);
        }

        private static ModelHandlers GetModelHandlers (EventCommand command, IModel model, bool create)
        {
            lock (_commandsHandlers) {
                if (create) {
                    ModelsHandlersDictionary modelsHandlers = _commandsHandlers.GetOrAdd(command, () => {
                        command.Subscribe(OnCommandExecute, OnCommandCanExecute);
                        return new ModelsHandlersDictionary();
                    });
                    return modelsHandlers.GetValue(model, k => new ModelHandlers());
                }
                else {
                    ModelsHandlersDictionary modelsHandlers = _commandsHandlers.GetOrDefault(command);
                    return modelsHandlers == null ? null : modelsHandlers.GetOrDefault(model);
                }
            }
        }

        public static EventCommand Register (Expression<Func<EventCommand>> propExpr, bool isAutoRequery = true)
        {
            return new EventCommand(Properties.GetName(propExpr), isAutoRequery);
        }

        internal class ModelHandlers
        {
            public event ExecuteEventHandler Execute;
            public event CanExecuteEventHandler CanExecute;

            public void RaiseExecute (object sender, ExecuteEventArgs args)
            {
                Execute.NullableInvoke(sender, args);
            }

            public void RaiseCanExecute (object sender, CanExecuteEventArgs args)
            {
                CanExecute.NullableInvoke(sender, args);
            }
        }
    }
}