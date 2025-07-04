﻿using System.Diagnostics.CodeAnalysis;
using Avalonia.Data.Converters;

namespace Alba.Framework.Avalonia.Converters;

public static class ConvertLogic
{
    [field: MaybeNull]
    public static IValueConverter IsNull => field ??= new IsNullConverter();

    [field: MaybeNull]
    public static IValueConverter NotNull => field ??= new NotNullConverter();

    [field: MaybeNull]
    public static IValueConverter ToBool => field ??= new ToBoolConverter();
}