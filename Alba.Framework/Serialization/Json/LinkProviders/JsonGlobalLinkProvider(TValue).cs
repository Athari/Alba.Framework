using System.Linq;
using Alba.Framework.Collections;
using Alba.Framework.Common;
using Alba.Framework.Text;
using Newtonsoft.Json;

namespace Alba.Framework.Serialization.Json
{
    public class JsonGlobalLinkProvider<TValue> : JsonUniqueLinkProviderBase<TValue>
        where TValue : IIdentifiable<string>
    {
        private readonly GlobalLinkData _linkData = new GlobalLinkData();

        public JsonGlobalLinkProvider (string idProp)
            : base(idProp)
        {}

        public override string GetLink (TValue value, JsonSerializer serializer, JsonLinkedContext context)
        {
            return _linkData.GetLink(value, serializer, context);
        }

        public override object ResolveOrigin (string id, JsonResolveLinkContext resolveContext)
        {
            return _linkData.ResolveOrigin(id, resolveContext);
        }

        public override object ResolveLink (string link, JsonResolveLinkContext resolveContext)
        {
            return _linkData.ResolveLink(link, resolveContext);
        }

        public override void ValidateLinksResolved ()
        {
            _linkData.ValidateLinksResolved();
        }

        protected class GlobalLinkData : LinkData
        {
            public override void ValidateLinksResolved ()
            {
                if (_unresolvedLinks.Any()) {
                    throw new JsonLinkProviderException("JSON global link provider for {0} contains unresolved links: '{1}'."
                        .Fmt(typeof(TValue).Name, _unresolvedLinks.JoinString("', '")));
                }
            }
        }
    }
}