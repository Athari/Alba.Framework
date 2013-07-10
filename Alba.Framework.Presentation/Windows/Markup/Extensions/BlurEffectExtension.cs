using System;
using System.Windows.Markup;
using System.Windows.Media.Effects;

namespace Alba.Framework.Windows.Markup.Extensions
{
    public class BlurEffectExtension : MarkupExtension
    {
        public BlurEffectExtension ()
        {
            KernelType = KernelType.Gaussian;
            Radius = 5;
            RenderingBias = RenderingBias.Performance;
        }

        public override object ProvideValue (IServiceProvider provider)
        {
            return new BlurEffect {
                KernelType = KernelType,
                Radius = Radius,
                RenderingBias = RenderingBias,
            };
        }

        public KernelType KernelType { get; set; }
        public double Radius { get; set; }
        public RenderingBias RenderingBias { get; set; }
    }
}