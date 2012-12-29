using System;
using System.Globalization;
using Alba.Framework.Sys;

namespace Alba.Framework.Common
{
    public class NamedObject
    {
        private string _name;

        public NamedObject (string name)
        {
            if (name.IsNullOrEmpty())
                throw new ArgumentNullException(name);
            _name = name;
        }

        public override string ToString ()
        {
            if (_name[0] != '{')
                _name = string.Format(CultureInfo.InvariantCulture, "{{{0}}}", _name);
            return _name;
        }
    }
}