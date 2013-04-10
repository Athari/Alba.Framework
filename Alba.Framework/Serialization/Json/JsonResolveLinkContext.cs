using System;
using Newtonsoft.Json;

namespace Alba.Framework.Serialization.Json
{
    public class JsonResolveLinkContext
    {
        private JsonLinkedContext _context;

        public Type Type { get; private set; }
        public JsonSerializer Serializer { get; private set; }
        public IJsonLineInfo LineInfo { get; private set; }

        public JsonResolveLinkContext (Type type, JsonSerializer serializer, IJsonLineInfo lineInfo)
        {
            Type = type;
            Serializer = serializer;
            LineInfo = lineInfo;
        }

        public JsonLinkedContext Context
        {
            get { return _context ?? (_context = JsonLinkedContext.Get(Serializer.Context)); }
        }

        public string IdProp
        {
            get { return Context.Options.GetLinkProvider(Type).IdProp; }
        }

        public void UpdateTypeFromTypeProperty (Type baseType, string typeName)
        {
            if (typeName == null)
                return;
            Type = Serializer.Binder.BindToType(baseType, typeName);
        }

        public object CreateEmpty (string link)
        {
            int typeSeparatorPos = link.IndexOf(JsonLinkedContext.LinkTypeSeparator);
            Type type = typeSeparatorPos == -1 ? Type : Serializer.Binder.BindToType(Type, link.Substring(0, typeSeparatorPos));
            return Serializer.Create(type);
        }
    }
}