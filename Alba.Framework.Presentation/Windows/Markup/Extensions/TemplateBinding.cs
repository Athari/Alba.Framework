using System.Windows.Data;

namespace Alba.Framework.Windows.Markup
{
    public class TemplateBinding : Binding
    {
        public TemplateBinding ()
        {
            RelativeSource = RelativeSource.TemplatedParent;
        }

        public TemplateBinding (string path) : base(path)
        {
            RelativeSource = RelativeSource.TemplatedParent;
        }

        public TemplateBinding (string path, IValueConverter converter) : base(path)
        {
            RelativeSource = RelativeSource.TemplatedParent;
            Converter = converter;
        }
    }
}