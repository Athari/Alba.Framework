using System.Collections;
using System.Globalization;

namespace Alba.Framework.Avalonia.Markup.Converters;

[PublicAPI]
public class ItemIndexConverter : MultiValueConverterBase<object?, IList?, int?>
{
    public int Offset { get; set; } = 1;
    public int? Default { get; set; }

    public override int? Convert(object? item, IList? list, Type targetType, CultureInfo culture)
    {
        var index = list?.IndexOf(item);
        return index is null or -1 ? Default : index + Offset;
    }
}