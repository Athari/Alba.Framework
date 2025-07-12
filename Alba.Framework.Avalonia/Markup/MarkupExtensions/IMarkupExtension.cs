using Avalonia.Markup.Xaml;

namespace Alba.Framework.Avalonia.Markup.MarkupExtensions;

/// <summary>Strict typing for duck-typed <see cref="MarkupExtension"/>.</summary>
[PublicAPI]
public interface IMarkupExtension
{
    object ProvideValue(IServiceProvider provider);
}

[PublicAPI]
public interface IMarkupExtension<out T>
{
    T ProvideValue(IServiceProvider provider);
}