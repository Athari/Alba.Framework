using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Markup.Xaml;

namespace Alba.Framework.Avalonia.Markup.Converters;

public class PrevNextEnabledConverter : MarkupExtension, IMultiValueConverter
{
    public PrevNext PrevNext { get; set; }

    public PrevNextEnabledConverter() { }
    public PrevNextEnabledConverter(PrevNext prevNext) => PrevNext = prevNext;

    public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
    {
        if (values is not [ int count, int index ])
            return false;
        return index == -1 || count > 0 && PrevNext switch {
            PrevNext.Prev => index > 0,
            PrevNext.Next => index < count - 1,
            _ => true,
        };
    }

    public override object ProvideValue(IServiceProvider serviceProvider) => this;
}

public enum PrevNext
{
    Prev,
    Next,
}