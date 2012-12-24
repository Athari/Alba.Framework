using System;
using System.Windows.Data;

namespace Alba.Framework.Markup
{
    public class AncestorBinding : Binding
    {
        public AncestorBinding (Type ancestor)
        {
            RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor, ancestor, 1);
        }

        public AncestorBinding (string path, Type ancestor) : base(path)
        {
            RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor, ancestor, 1);
        }
    }
}