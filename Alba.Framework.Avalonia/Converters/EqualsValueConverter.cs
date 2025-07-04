using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;
using Avalonia.Markup.Xaml;

namespace Alba.Framework.Avalonia.Converters;

public class EqualsValueConverter : MarkupExtension, IValueConverter
{
    public object? Value { get; set; }

    public EqualsValueConverter() { }

    public EqualsValueConverter(object value) =>
        Value = value;

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        Equals(value, Value);

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        Equals(value, true) ? Value : AvaloniaProperty.UnsetValue;

    public override object ProvideValue(IServiceProvider serviceProvider) =>
        this;
}