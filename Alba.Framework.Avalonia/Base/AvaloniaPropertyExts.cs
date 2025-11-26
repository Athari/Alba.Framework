using Avalonia;
using Avalonia.Data;
using Avalonia.Reactive;

namespace Alba.Framework.Avalonia;

[SuppressMessage("AvaloniaProperty", "AVP1001: The same AvaloniaProperty should not be registered twice", Justification = "Simple wrapper")]
public static class AvaloniaPropertyExts
{
    extension(AvaloniaProperty @this)
    {
        /// <inheritdoc cref="AvaloniaProperty.Register{TOwner,TValue}" />
        public static StyledProperty<TValue> Property<TOwner, TValue>(
            string name,
            TValue defaultValue = default!,
            bool inherits = false,
            BindingMode defaultBindingMode = BindingMode.OneWay,
            Func<TValue, bool>? validate = null,
            Func<AvaloniaObject, TValue, TValue>? coerce = null,
            bool enableDataValidation = false,
            Func<TOwner, Action<AvaloniaPropertyChangedEventArgs<TValue>>>? changed = null)
            where TOwner : AvaloniaObject
        {
            var prop = AvaloniaProperty.Register<TOwner, TValue>(
                name, defaultValue, inherits, defaultBindingMode, validate, coerce, enableDataValidation);
            if (changed != null)
                prop.Changed.Subscribe(changed);
            return prop;
        }

        /// <inheritdoc cref="AvaloniaProperty.RegisterAttached{TOwner,THost,TValue}" />
        public static AttachedProperty<TValue> AttachedProperty<TOwner, THost, TValue>(
            string name,
            TValue defaultValue = default!,
            bool inherits = false,
            BindingMode defaultBindingMode = BindingMode.OneWay,
            Func<TValue, bool>? validate = null,
            Func<AvaloniaObject, TValue, TValue>? coerce = null,
            Func<TOwner, Action<AvaloniaPropertyChangedEventArgs<TValue>>>? changed = null)
            where THost : AvaloniaObject
        {
            var property = AvaloniaProperty.RegisterAttached<TOwner, THost, TValue>(
                name, defaultValue, inherits, defaultBindingMode, validate, coerce);
            if (changed != null)
                property.Changed.Subscribe(changed);
            return property;
        }

        /// <inheritdoc cref="AvaloniaProperty.RegisterDirect{TOwner,TValue}" />
        public static DirectProperty<TOwner, TValue> DirectProperty<TOwner, TValue>(
            string name,
            Func<TOwner, TValue> getter,
            Action<TOwner, TValue>? setter = null,
            TValue unsetValue = default!,
            BindingMode defaultBindingMode = BindingMode.OneWay,
            bool enableDataValidation = false)
            where TOwner : AvaloniaObject =>
            AvaloniaProperty.RegisterDirect(
                name, getter, setter, unsetValue, defaultBindingMode, enableDataValidation);
    }

    extension<T>(IObservable<AvaloniaPropertyChangedEventArgs<T>> @this)
    {
        public IDisposable Subscribe<TSender>(Action<TSender, AvaloniaPropertyChangedEventArgs<T>> action) =>
            @this.Subscribe(
                new AnonymousObserver<AvaloniaPropertyChangedEventArgs<T>>(e => {
                    if (e.Sender is not TSender sender)
                        return;
                    action(sender, e);
                })
            );

        public IDisposable Subscribe<TSender>(Func<TSender, Action<AvaloniaPropertyChangedEventArgs<T>>> action)
        {
            return @this.Subscribe((TSender o, AvaloniaPropertyChangedEventArgs<T> e) => action(o)(e));
        }
    }
}