﻿using Alba.Framework.Collections;
using Avalonia.Data;
using AvaloniaMultiBinding = Avalonia.Data.MultiBinding;

namespace Alba.Framework.Avalonia.Markup.MarkupExtensions;

[PublicAPI]
public class MultiBinding : AvaloniaMultiBinding, IMarkupExtension<MultiBinding>
{
    public MultiBinding(IBinding b1, IBinding b2) =>
        Bindings.AddRange([ b1, b2 ]);

    public MultiBinding(IBinding b1, IBinding b2, IBinding b3) =>
        Bindings.AddRange([ b1, b2, b3 ]);

    public MultiBinding(IBinding b1, IBinding b2, IBinding b3, IBinding b4) =>
        Bindings.AddRange([ b1, b2, b3, b4 ]);

    public MultiBinding ProvideValue(IServiceProvider provider) => this;
}