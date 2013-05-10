using System.Linq;
using Alba.Framework.Collections;
using Alba.Framework.Common;
using Alba.Framework.Text;
using Newtonsoft.Json;

namespace Alba.Framework.Serialization.Json
{
    public class PathLinkProvider<TValue, TRoot> : PathLinkProviderBase<TValue, TRoot>
        where TValue : class, IIdentifiable<string>
        where TRoot : class
    {
        private readonly GlobalLinkData _linkData = new GlobalLinkData();

        public PathLinkProvider (string idProp) :
            base(idProp)
        {}

        public override string GetLink (TValue value, JsonSerializer serializer, JsonLinkedContext context)
        {
            return _linkData.GetLink(value);
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

        public override void RememberOriginLink (TValue value, JsonLinkedContext context)
        {
            _linkData.RememberOriginLink(value, context);
        }

        private class GlobalLinkData : LinkData
        {
            public override void ValidateLinksResolved ()
            {
                if (_unresolvedLinks.Any()) {
                    throw new JsonLinkProviderException("JSON global path link provider for {0} (root={1}) contains unresolved links: '{2}'."
                        .Fmt(typeof(TValue).Name, typeof(TRoot).Name, _unresolvedLinks.JoinString("', '")));
                }
            }
        }
    }
}