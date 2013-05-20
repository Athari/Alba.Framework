using System;
using System.Windows.Media;

namespace Alba.Framework.Wpf
{
    public class HslColor
    {
        private float _alpha, _hue, _saturation, _lightness;

        public HslColor (float alpha, float hue, float saturation, float lightness)
        {
            Alpha = alpha;
            Hue = hue;
            Saturation = saturation;
            Lightness = lightness;
        }

        public HslColor (float hue, float saturation, float lightness)
        {
            Alpha = 1;
            Hue = hue;
            Saturation = saturation;
            Lightness = lightness;
        }

        public HslColor (double alpha, double hue, double saturation, double lightness)
        {
            Alpha = (float)alpha;
            Hue = (float)hue;
            Saturation = (float)saturation;
            Lightness = (float)lightness;
        }

        public HslColor (double hue, double saturation, double lightness)
        {
            Alpha = 1;
            Hue = (float)hue;
            Saturation = (float)saturation;
            Lightness = (float)lightness;
        }

        public static HslColor From255 (int alpha, int hue, int saturation, int lightness)
        {
            return new HslColor {
                Alpha = alpha / 255f,
                Hue = hue / 255f,
                Saturation = saturation / 255f,
                Lightness = lightness / 255f,
            };
        }

        public static HslColor From255 (int hue, int saturation, int lightness)
        {
            return new HslColor {
                Alpha = 1,
                Hue = hue / 255f,
                Saturation = saturation / 255f,
                Lightness = lightness / 255f,
            };
        }

        public HslColor ()
        {
            _alpha = 1;
        }

        public HslColor (Color rgb)
        {
            FromColor(rgb);
        }

        public float Alpha
        {
            get { return _alpha; }
            set
            {
                if (value < 0)
                    _alpha = 0;
                else if (value > 1)
                    _alpha = 1;
                else
                    _alpha = value;
            }
        }

        public float Hue
        {
            get { return _hue; }
            set
            {
                if (value < 0)
                    _hue = 0;
                else if (value > 1)
                    _hue = 1;
                else
                    _hue = value;
            }
        }

        public float Saturation
        {
            get { return _saturation; }
            set
            {
                if (value < 0)
                    _saturation = 0;
                else if (value > 1)
                    _saturation = 1;
                else
                    _saturation = value;
            }
        }

        public float Lightness
        {
            get { return _lightness; }
            set
            {
                if (value < 0)
                    _lightness = 0;
                else if (value > 1)
                    _lightness = 1;
                else
                    _lightness = value;
            }
        }

        public HslColor Darker (float percent)
        {
            return new HslColor(_alpha, _hue, _saturation, _lightness * (1 - percent));
        }

        public HslColor Lighter (float percent)
        {
            return new HslColor(_alpha, _hue, _saturation, _lightness * (1 + percent));
        }

        private void FromColor (Color rgb)
        {
            _alpha = rgb.ScA;

            float max = Math.Max(Math.Max(rgb.ScR, rgb.ScG), rgb.ScB);
            float min = Math.Min(Math.Min(rgb.ScR, rgb.ScG), rgb.ScB);

            _lightness = (min + max) / 2;
            if (_lightness <= 0.0)
                return;

            float delta = max - min;
            _saturation = delta;
            if (_saturation <= 0)
                return;
            _saturation = (float)(delta / _lightness <= 0.5 ? max + min : 2.0 - max - min);

            float r2 = (max - rgb.ScR) / delta;
            float g2 = (max - rgb.ScG) / delta;
            float b2 = (max - rgb.ScB) / delta;

            // ReSharper disable CompareOfFloatsByEqualityOperator
            if (rgb.ScR == max)
                _hue = (rgb.ScG == min ? 5 + b2 : 1 - g2) / 6;
            else if (rgb.ScG == max)
                _hue = (rgb.ScB == min ? 1 + r2 : 3 - b2) / 6;
            else
                _hue = (rgb.ScR == min ? 3 + g2 : 5 - r2) / 6;
            // ReSharper restore CompareOfFloatsByEqualityOperator
        }

        public Color ToColor ()
        {
            double h = _hue * 6, s = _saturation, l = _lightness;
            double r = l, g = l, b = l, v = (l <= 0.5) ? (l * (1.0 + s)) : (l + s - l * s);

            if (v > 0) {
                int sextant = (int)h;
                double m = 2 * l - v, vsf = v * ((v - m) / v) * (h - sextant);
                switch (sextant) {
                    case 0:
                    case 6:
                        r = v;
                        g = m + vsf;
                        b = m;
                        break;
                    case 1:
                        r = v - vsf;
                        g = v;
                        b = m;
                        break;
                    case 2:
                        r = m;
                        g = v;
                        b = m + vsf;
                        break;
                    case 3:
                        r = m;
                        g = v - vsf;
                        b = v;
                        break;
                    case 4:
                        r = m + vsf;
                        g = m;
                        b = v;
                        break;
                    case 5:
                        r = v;
                        g = m;
                        b = v - vsf;
                        break;
                }
            }
            return new Color { ScA = _alpha, ScR = (float)r, ScG = (float)g, ScB = (float)b };
        }
    }
}