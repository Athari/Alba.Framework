using System;
using Newtonsoft.Json;

namespace Alba.Framework.Serialization.Json
{
    public interface IJsonLinkProvider
    {
        string IdProp { get; }

        bool CanLink (Type type);
        string GetLink (object value, JsonSerializer serializer, JsonLinkedContext context);
        object ResolveOrigin (string id, JsonResolveLinkContext resolveContext);
        object ResolveLink (string link, JsonResolveLinkContext resolveContext);
        void RememberOriginLink (object value, JsonLinkedContext context);
        void ValidateLinksResolved ();
    }
}