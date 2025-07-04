using System.Runtime.CompilerServices;

namespace Alba.Framework.Threading.Tasks;

[PublicAPI]
public static class TaskExts
{
    private static T ThisNotNull<T>(this T? @this)
    {
        Guard.IsNotNull(@this);
        return @this;
    }

    public static ConfiguredTaskAwaitable NoSync(this Task @this) =>
        ThisNotNull(@this).ConfigureAwait(false);

    public static ConfiguredTaskAwaitable<T> NoSync<T>(this Task<T> @this) =>
        ThisNotNull(@this).ConfigureAwait(false);

    public static ConfiguredAsyncDisposable NoSync<T>(this T @this) where T : IAsyncDisposable =>
        ThisNotNull(@this).ConfigureAwait(false);

    public static ConfiguredAsyncDisposable AsNoSync<T>(this T @this, out T var) where T : IAsyncDisposable =>
        ThisNotNull(@this).As(out var).ConfigureAwait(false);

    public static ConfiguredValueTaskAwaitable NoSync(this ValueTask @this) =>
        @this.ConfigureAwait(false);

    public static ConfiguredValueTaskAwaitable<T> NoSync<T>(this ValueTask<T> @this) =>
        @this.ConfigureAwait(false);

    public static void NoAwait(this Task _) { }

    public static void NoAwait<T>(this Task<T> _) { }

    public static void NoAwait(this ValueTask _) { }

    public static void NoAwait<T>(this ValueTask<T> _) { }

    public static Task OrCompleted(this Task? @this) =>
        @this ?? Task.CompletedTask;

    public static Task<T> OrCompleted<T>(this Task<T>? @this, T defaultValue = default!) =>
        @this ?? Task.FromResult(defaultValue);

    public static ValueTask OrCompleted(this ValueTask? @this) =>
        @this ?? ValueTask.CompletedTask;

    public static ValueTask<T> OrCompleted<T>(this ValueTask<T>? @this, T defaultValue = default!) =>
        @this ?? ValueTask.FromResult(defaultValue);

    public static IEnumerable<T> Yield<T>(this T @this)
    {
        yield return @this;
    }
}