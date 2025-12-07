using Avalonia.Data;

namespace Alba.Framework.Avalonia.Markup.MarkupExtensions;

public abstract class ParamBinding<TParam> : MultiBinding
{
    protected ParamBinding(IBinding b1, TParam param) : base(b1) => ConverterParameter = param;
}