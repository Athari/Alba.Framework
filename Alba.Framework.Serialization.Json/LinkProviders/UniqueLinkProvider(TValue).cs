using System.Linq;
using Alba.Framework.Collections;
using Alba.Framework.Common;
using Alba.Framework.Text;

namespace Alba.Framework.Serialization.Json
{
    public class UniqueLinkProvider<TValue> : UniqueLinkProviderBase<TValue>
        where TValue : IIdentifiable<string>
    {
        private readonly GlobalLinkData _linkData = new GlobalLinkData();

        public UniqueLinkProvider (string idProp)
            : base(idProp)
        {}

        public override string GetLink (TValue value, JsonLinkedContext context)
        {
            return _linkData.GetLink(value, context);
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
                    throw new JsonLinkProviderException("JSON global unique link provider for {0} contains unresolved links: '{1}'."
                        .Fmt(typeof(TValue).Name, _unresolvedLinks.JoinString("', '")));
                }
            }
        }
    }
}