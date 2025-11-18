using System.Drawing;
using Avalonia.Animation;
using Avalonia.Animation.Easings;
using Avalonia.Markup.Xaml.Converters;
using Avalonia.Media;
using Avalonia.Styling;
using DrawingColor = System.Drawing.Color;
using DrawingColorConverter = System.Drawing.ColorConverter;

namespace Alba.Framework.Avalonia.Json;

// System.Drawing - TypeConverter

public class DrawingColorJsonConverter : TypeJsonConverter<DrawingColor, string, DrawingColorConverter>;

public class DrawingPointJsonConverter : TypeJsonConverter<Point, string, PointConverter>;

public class DrawingRectangleJsonConverter : TypeJsonConverter<Rectangle, string, RectangleConverter>;

public class DrawingSizeJsonConverter : TypeJsonConverter<Size, string, SizeConverter>;

public class DrawingSizeFJsonConverter : TypeJsonConverter<SizeF, string, SizeFConverter>;

// Avalonia - TypeConverter

public class AvaloniaUriJsonConverter : TypeJsonConverter<Uri, string, AvaloniaUriTypeConverter>;

public class BrushJsonConverter : TypeJsonConverter<Brush, string, BrushConverter>;

public class CueJsonConverter : TypeJsonConverter<Cue, string, CueTypeConverter>;

public class EasingJsonConverter : TypeJsonConverter<Easing, string, EasingTypeConverter>;

public class EffectJsonConverter : TypeJsonConverter<Effect, string, EffectConverter>;

public class FontFamilyJsonConverter : TypeJsonConverter<FontFamily, string, FontFamilyTypeConverter>;

public class GeometryJsonConverter : TypeJsonConverter<Geometry, string, GeometryTypeConverter>;

public class IterationCountJsonConverter : TypeJsonConverter<IterationCount, string, IterationCountTypeConverter>;

public class KeySplineJsonConverter : TypeJsonConverter<KeySpline, string, KeySplineTypeConverter>;

public class ThemeVariantJsonConverter : TypeJsonConverter<ThemeVariant, string, ThemeVariantTypeConverter>;