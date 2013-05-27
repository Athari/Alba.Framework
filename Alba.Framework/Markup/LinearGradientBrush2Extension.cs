using System;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;

namespace Alba.Framework.Markup
{
    public class LinearGradientBrush2Extension : MarkupExtension
    {
        public Color Color1 { get; set; }
        public Color Color2 { get; set; }
        public double Angle { get; set; }

        public LinearGradientBrush2Extension (Color color1, Color color2) : this(color1, color2, 0)
        {}

        public LinearGradientBrush2Extension (Color color1, Color color2, double angle)
        {
            Color1 = color1;
            Color2 = color2;
            Angle = angle;
        }

        public override object ProvideValue (IServiceProvider provider)
        {
            return new LinearGradientBrush {
                EndPoint = EndPointFromAngle(Angle),
                GradientStops = {
                    new GradientStop(Color1, 0),
                    new GradientStop(Color2, 1),
                }
            };
        }

        private static Point EndPointFromAngle (double angle)
        {
            angle = angle * (1.0 / 180.0) * Math.PI;
            return new Point(Math.Cos(angle), Math.Sin(angle));
        }
    }
}