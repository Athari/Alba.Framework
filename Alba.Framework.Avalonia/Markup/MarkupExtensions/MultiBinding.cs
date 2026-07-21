using System.ComponentModel;
using System.Globalization;
using Alba.Framework.Collections;
using Avalonia;
using Avalonia.Data;
using Avalonia.Data.Converters;
using Avalonia.Metadata;
using AvaloniaMultiBinding = Avalonia.Data.MultiBinding;

namespace Alba.Framework.Avalonia.Markup.MarkupExtensions;

public class MultiBinding : IMarkupExtension<BindingBase>
{
    public MultiBinding(BindingBase b1) =>
        Bindings.AddRange([ b1 ]);

    public MultiBinding(BindingBase b1, BindingBase b2) =>
        Bindings.AddRange([ b1, b2 ]);

    public MultiBinding(BindingBase b1, BindingBase b2, BindingBase b3) =>
        Bindings.AddRange([ b1, b2, b3 ]);

    public MultiBinding(BindingBase b1, BindingBase b2, BindingBase b3, BindingBase b4) =>
        Bindings.AddRange([ b1, b2, b3, b4 ]);

    [Content, AssignBinding]
    public IList<BindingBase> Bindings { get; set; } = new List<BindingBase>();


    [TypeConverter(typeof(CultureInfoIetfLanguageTagConverter))]
    public CultureInfo? ConverterCulture { get; set; }

    public IMultiValueConverter? Converter { get; set; }
    public object? ConverterParameter { get; set; }
    public object FallbackValue { get; set; } = AvaloniaProperty.UnsetValue;
    public object TargetNullValue { get; set; } = AvaloniaProperty.UnsetValue;
    public BindingMode Mode { get; set; } = BindingMode.OneWay;
    public BindingPriority Priority { get; set; }
    public RelativeSource? RelativeSource { get; set; }
    public string? StringFormat { get; set; }

    public BindingBase ProvideValue(IServiceProvider provider)
    {
        return new AvaloniaMultiBinding {
            Bindings = Bindings,
            Converter = Converter,
            ConverterCulture = ConverterCulture,
            ConverterParameter = ConverterParameter,
            FallbackValue = FallbackValue,
            Mode = Mode,
            Priority = Priority,
            RelativeSource = RelativeSource,
            StringFormat = StringFormat,
            TargetNullValue = TargetNullValue,
        };
    }
}