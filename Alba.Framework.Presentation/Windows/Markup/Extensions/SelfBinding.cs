using System.Windows.Data;

namespace Alba.Framework.Windows.Markup
{
    public class SelfBinding : Binding
    {
        public SelfBinding ()
        {
            RelativeSource = RelativeSource.Self;
        }

        public SelfBinding (string path) : base(path)
        {
            RelativeSource = RelativeSource.Self;
        }

        public SelfBinding (string path, IValueConverter converter) : base(path)
        {
            RelativeSource = RelativeSource.Self;
            Converter = converter;
        }
    }
}