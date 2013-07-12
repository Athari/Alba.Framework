using System;
using System.Reflection;
using System.Windows;
using System.Windows.Data;
using Alba.Framework.Text;

namespace Alba.Framework.Windows.Markup
{
    public class AncestorBinding : Binding
    {
        public AncestorBinding (string path) : base(path)
        {
            if (!IsPropertyReference(path))
                throw new ArgumentException("Ancestor type not specified in path '{0}'.".Fmt(path), "path");
            string propRef = path.Substring(1, path.IndexOf(')') - 1);
            string ownerTypeName = propRef.Substring(0, propRef.LastIndexOf('.')).Trim();
            Type ownerType = GetTypeFromName(Path, ownerTypeName);
            if (ownerType == null)
                throw new ArgumentException("Ancestor type '{0}' not found using path '{1}'.".Fmt(ownerTypeName, path), "path");
            RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor, ownerType, 1);
        }

        public AncestorBinding (string path, Type ancestor) : base(path)
        {
            RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor, ancestor, 1);
        }

        private static bool IsPropertyReference (string name)
        {
            return name != null && name.Length > 1 && name[0] == '(' && name.IndexOf('.') != -1;
        }

        private static Type GetTypeFromName (PropertyPath propertyPath, string name)
        {
            // HACK
            return (Type)typeof(PropertyPath)
                .GetMethod("GetTypeFromName", BindingFlags.Instance | BindingFlags.NonPublic)
                .Invoke(propertyPath, new object[] { name, null });
        }
    }
}