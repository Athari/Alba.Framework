using Avalonia.Markup.Xaml;

namespace Alba.Framework.Avalonia.Markup.MarkupExtensions;

/// <summary>Strict typing for duck-typed <see cref="MarkupExtension"/>.</summary>
public interface IMarkupExtension
{
    object ProvideValue(IServiceProvider provider);
}

public interface IMarkupExtension<out T>
{
    T ProvideValue(IServiceProvider provider);
}