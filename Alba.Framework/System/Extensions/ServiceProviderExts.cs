using Alba.Framework.Reflection;

namespace Alba.Framework;

[PublicAPI]
public static class ServiceProviderExts
{
    public static T GetService<T>(this IServiceProvider @this)
        where T : class =>
        @this.GetService(typeof(T)) as T ?? throw new ArgumentException($"Service {typeof(T).GetFullName()} not supported", nameof(T));

    public static T? TryGetService<T>(this IServiceProvider @this)
        where T : class =>
        @this.GetService(typeof(T)) as T;
}