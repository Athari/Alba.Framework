using System.ComponentModel;
using System.Windows.Markup;

namespace Alba.Framework.Windows.Input
{
    public class AnyKeyGestureValueSerializer : ValueSerializer
    {
        private static readonly TypeConverter _converter = new AnyKeyGestureConverter();

        public override bool CanConvertFromString (string value, IValueSerializerContext context)
        {
            return true;
        }

        public override bool CanConvertToString (object value, IValueSerializerContext context)
        {
            return value is AnyKeyGesture;
        }

        public override object ConvertFromString (string value, IValueSerializerContext context)
        {
            return _converter.ConvertFromString(value);
        }

        public override string ConvertToString (object value, IValueSerializerContext context)
        {
            return _converter.ConvertToInvariantString(value);
        }
    }
}