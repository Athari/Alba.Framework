using Alba.Framework.Reflection;

namespace Alba.Framework;

public static class ServiceProviderExts
{
    [field: MaybeNull]
    public static IServiceProvider Empty => field ??= new EmptyServiceProvider();

    extension(IServiceProvider @this)
    {
        public T GetService<T>()
            where T : class =>
            @this.GetService(typeof(T)) as T ?? throw new ArgumentException($"Service {typeof(T).GetFullName()} not supported", nameof(T));

        public T? TryGetService<T>()
            where T : class =>
            @this.GetService(typeof(T)) as T;
    }

    private sealed class EmptyServiceProvider : IServiceProvider
    {
        public object? GetService(Type serviceType) => null;
    }
}