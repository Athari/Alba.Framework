using System;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace Alba.Framework.Markup
{
    public class DropShadowEffectExtension : MarkupExtension
    {
        public double BlurRadius { get; set; }
        public Color Color { get; set; }
        public double Direction { get; set; }
        public double Opacity { get; set; }
        public RenderingBias RenderingBias { get; set; }
        public double ShadowDepth { get; set; }

        public DropShadowEffectExtension ()
        {
            BlurRadius = 5;
            Color = Colors.Black;
            Direction = 315;
            Opacity = 1;
            RenderingBias = RenderingBias.Performance;
            ShadowDepth = 5;
        }

        public override object ProvideValue (IServiceProvider provider)
        {
            return new DropShadowEffect {
                BlurRadius = BlurRadius,
                Color = Color,
                Direction = Direction,
                Opacity = Opacity,
                RenderingBias = RenderingBias,
                ShadowDepth = ShadowDepth,
            };
        }
    }
}