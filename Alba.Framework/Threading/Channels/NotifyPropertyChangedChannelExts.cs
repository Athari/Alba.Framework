using System.ComponentModel;
using System.Linq.Expressions;
using System.Threading.Channels;
using Alba.Framework.Reflection;
using Alba.Framework.Threading.Tasks;

namespace Alba.Framework.Threading.Channels;

public static class NotifyPropertyChangedChannelExts
{
    extension<TObj>(TObj @this) where TObj : INotifyPropertyChanged
    {
        public ChannelReader<T> PropertyChangedToBoundedChannel<T>(
            Expression<Func<TObj, T>> prop,
            BoundedChannelOptions options, CancellationToken ct)
        {
            var getter = prop.Compile();
            return @this.PropertyChangedToBoundedChannel(prop, () => getter(@this), options, ct);
        }

        public ChannelReader<T> PropertyChangedToUnboundedChannel<T>(
            Expression<Func<TObj, T>> prop,
            UnboundedChannelOptions? options, CancellationToken ct)
        {
            var getter = prop.Compile();
            return @this.PropertyChangedToUnboundedChannel(prop, () => getter(@this), options, ct);
        }

        public ChannelReader<T> PropertyChangedToUnboundedPrioritizedChannel<T>(
            Expression<Func<TObj, T>> prop,
            UnboundedPrioritizedChannelOptions<T>? options, CancellationToken ct)
        {
            var getter = prop.Compile();
            return @this.PropertyChangedToUnboundedPrioritizedChannel(prop, () => getter(@this), options, ct);
        }
    }

    extension(INotifyPropertyChanged @this)
    {
        public ChannelReader<T> PropertyChangedToBoundedChannel<T>(
            string[] propertyNames, Func<T> getValue,
            BoundedChannelOptions options, CancellationToken ct)
        {
            options.SingleWriter = true;
            var channel = Channel.CreateBounded<T>(options);
            return @this.SetupChannel(propertyNames, getValue, channel, ct);
        }

        public ChannelReader<T> PropertyChangedToUnboundedChannel<T>(
            string[] propertyNames, Func<T> getValue,
            UnboundedChannelOptions? options, CancellationToken ct)
        {
            options ??= new();
            options.SingleWriter = true;
            var channel = Channel.CreateUnbounded<T>(options);
            return @this.SetupChannel(propertyNames, getValue, channel, ct);
        }

        public ChannelReader<T> PropertyChangedToUnboundedPrioritizedChannel<T>(
            string[] propertyNames, Func<T> getValue,
            UnboundedPrioritizedChannelOptions<T>? options, CancellationToken ct)
        {
            options ??= new();
            options.SingleWriter = true;
            var channel = Channel.CreateUnboundedPrioritized(options);
            return @this.SetupChannel(propertyNames, getValue, channel, ct);
        }

        private ChannelReader<T> PropertyChangedToBoundedChannel<T>(
            LambdaExpression prop, Func<T> getValue,
            BoundedChannelOptions options, CancellationToken ct)
        {
            options.SingleWriter = true;
            var channel = Channel.CreateBounded<T>(options);
            return @this.SetupChannel(prop, getValue, channel, ct);
        }

        private ChannelReader<T> PropertyChangedToUnboundedChannel<T>(
            LambdaExpression prop, Func<T> getValue,
            UnboundedChannelOptions? options, CancellationToken ct)
        {
            options ??= new();
            options.SingleWriter = true;
            var channel = Channel.CreateUnbounded<T>(options);
            return @this.SetupChannel(prop, getValue, channel, ct);
        }

        private ChannelReader<T> PropertyChangedToUnboundedPrioritizedChannel<T>(
            LambdaExpression prop, Func<T> getValue,
            UnboundedPrioritizedChannelOptions<T>? options, CancellationToken ct)
        {
            options ??= new();
            options.SingleWriter = true;
            var channel = Channel.CreateUnboundedPrioritized(options);
            return @this.SetupChannel(prop, getValue, channel, ct);
        }

        private ChannelReader<T> SetupChannel<T>(
            LambdaExpression prop, Func<T> getValue,
            Channel<T> channel, CancellationToken ct)
        {
            return @this.SetupChannel([ PropertyName.Of(prop) ], getValue, channel, ct);
        }

        private ChannelReader<T> SetupChannel<T>(
            string[] propertyNames, Func<T> getValue,
            Channel<T> channel, CancellationToken ct)
        {
            Guard.IsNotDefault(ct);

            @this.PropertyChanged += OnPropertyChanged;
            ct.Register(() => {
                @this.PropertyChanged -= OnPropertyChanged;
                channel.Writer.Complete();
            });
            channel.Reader.Completion.ContinueWith(_ =>
                @this.PropertyChanged -= OnPropertyChanged, ct);
            return channel.Reader;

            void OnPropertyChanged(object? _, PropertyChangedEventArgs e)
            {
                if (e.PropertyName == null || propertyNames.Contains(e.PropertyName))
                    channel.Writer.WriteAsync(getValue(), ct).NoWait();
            }
        }
    }
}