using Avalonia.Data;
using Avalonia.Data.Converters;

namespace Alba.Framework.Avalonia.Markup.MarkupExtensions;

public class OrExtension : MultiBinding
{
    public OrExtension(BindingBase b1, BindingBase b2) : base(b1, b2) => Converter = BoolConverters.Or;
    public OrExtension(BindingBase b1, BindingBase b2, BindingBase b3) : base(b1, b2, b3) => Converter = BoolConverters.Or;
    public OrExtension(BindingBase b1, BindingBase b2, BindingBase b3, BindingBase b4) : base(b1, b2, b3, b4) => Converter = BoolConverters.Or;
}