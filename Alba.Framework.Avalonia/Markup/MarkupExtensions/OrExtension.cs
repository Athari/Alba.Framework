using Avalonia.Data;
using Avalonia.Data.Converters;

namespace Alba.Framework.Avalonia.Markup.MarkupExtensions;

public class OrExtension : MultiBinding
{
    public OrExtension(IBinding b1, IBinding b2) : base(b1, b2) => Converter = BoolConverters.Or;
    public OrExtension(IBinding b1, IBinding b2, IBinding b3) : base(b1, b2, b3) => Converter = BoolConverters.Or;
    public OrExtension(IBinding b1, IBinding b2, IBinding b3, IBinding b4) : base(b1, b2, b3, b4) => Converter = BoolConverters.Or;
}