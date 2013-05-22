using System;

namespace Alba.Framework.Serialization.Json
{
    public interface IJsonLinkProvider
    {
        string IdProp { get; }
        bool IsScoped { get; }

        bool CanLink (Type type);
        string GetLink (object value, JsonLinkedContext context);
        object ResolveOrigin (string id, JsonResolveLinkContext resolveContext);
        object ResolveLink (string link, JsonResolveLinkContext resolveContext);
        void RememberOriginLink (object value, JsonLinkedContext context);
        void ValidateLinksResolved ();
    }
}