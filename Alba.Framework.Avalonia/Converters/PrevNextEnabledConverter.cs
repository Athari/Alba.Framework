using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Markup.Xaml;

namespace Alba.Framework.Avalonia.Converters;

public class PrevNextEnabledConverter : MarkupExtension, IMultiValueConverter
{
    public PrevNext PrevNext { get; set; }

    public PrevNextEnabledConverter() { }
    public PrevNextEnabledConverter(PrevNext prevNext) => PrevNext = prevNext;

    public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
    {
        if (values is not [ IReadOnlyList<object> items, int index ])
            return false;
        return items.Count > 1 && PrevNext switch {
            PrevNext.Prev => index < items.Count - 1,
            PrevNext.Next => index > 1,
            _ => throw new ArgumentOutOfRangeException(),
        };
    }

    public override object ProvideValue(IServiceProvider serviceProvider) => this;
}

public enum PrevNext
{
    Prev,
    Next,
}