using System.Collections.Generic;
using System.Threading;
using System.Windows.Data;
using Alba.Framework.Collections;
using Newtonsoft.Json.Linq;

// ReSharper disable CheckNamespace
namespace Alba.Framework.Windows.Markup.Json
{
    public class JsonBinding : Binding
    {
        private static readonly ThreadLocal<IDictionary<JTokenType, IValueConverter>> _converters =
            new ThreadLocal<IDictionary<JTokenType, IValueConverter>>(
                () => new SortedList<JTokenType, IValueConverter>());

        public JsonBinding ()
        {
            Init();
        }

        public JsonBinding (string path) : base(path)
        {
            Init();
        }

        private void Init ()
        {
            TokenType = JTokenType.String;
        }

        public JTokenType TokenType
        {
            get { return ((JsonTokenConverter)Converter).TokenType; }
            set { Converter = _converters.Value.GetOrAdd(value, () => new JsonTokenConverter { TokenType = value }); }
        }
    }
}