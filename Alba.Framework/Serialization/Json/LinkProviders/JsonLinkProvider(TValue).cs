using System;
using Alba.Framework.Common;
using Newtonsoft.Json;

namespace Alba.Framework.Serialization.Json
{
    public abstract class JsonLinkProvider<TValue> : IJsonLinkProvider
        where TValue : IIdentifiable<string>
    {
        public string IdProp { get; private set; }

        protected JsonLinkProvider (string idProp)
        {
            IdProp = idProp;
        }

        protected static string GetUntypedLink (string link)
        {
            int typeSeparatorPos = link.IndexOf(JsonLinkedContext.LinkTypeSeparator);
            return typeSeparatorPos == -1 ? link : link.Substring(typeSeparatorPos + 1);
        }

        public virtual bool CanLink (Type type)
        {
            return typeof(TValue).IsAssignableFrom(type);
        }

        string IJsonLinkProvider.GetLink (object value, JsonSerializer serializer, JsonLinkedContext context)
        {
            return GetLink((TValue)value, serializer, context);
        }

        public abstract string GetLink (TValue value, JsonSerializer serializer, JsonLinkedContext context);

        public abstract object ResolveOrigin (string id, JsonResolveLinkContext resolveContext);

        public abstract object ResolveLink (string link, JsonResolveLinkContext resolveContext);

        void IJsonLinkProvider.RememberOriginLink (object value, JsonLinkedContext context)
        {
            RememberOriginLink((TValue)value, context);
        }

        public abstract void ValidateLinksResolved ();

        public virtual void RememberOriginLink (TValue value, JsonLinkedContext context)
        {}
    }
}