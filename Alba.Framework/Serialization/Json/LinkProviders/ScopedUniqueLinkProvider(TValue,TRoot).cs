using System.Collections.Generic;
using System.Linq;
using Alba.Framework.Collections;
using Alba.Framework.Common;
using Alba.Framework.Text;
using Newtonsoft.Json;

namespace Alba.Framework.Serialization.Json
{
    public class ScopedUniqueLinkProvider<TValue, TRoot> : UniqueLinkProviderBase<TValue>
        where TValue : IIdentifiable<string>
        where TRoot : class
    {
        private readonly IDictionary<TRoot, RootLinkData> _roots = new Dictionary<TRoot, RootLinkData>();

        public ScopedUniqueLinkProvider (string idProp)
            : base(idProp)
        {}

        public override string GetLink (TValue value, JsonSerializer serializer, JsonLinkedContext context)
        {
            return GetRootLinkData(context).GetLink(value, serializer, context);
        }

        public override object ResolveOrigin (string id, JsonResolveLinkContext resolveContext)
        {
            return GetRootLinkData(resolveContext.Context).ResolveOrigin(id, resolveContext);
        }

        public override object ResolveLink (string link, JsonResolveLinkContext resolveContext)
        {
            return GetRootLinkData(resolveContext.Context).ResolveLink(link, resolveContext);
        }

        public override void ValidateLinksResolved ()
        {
            foreach (RootLinkData linkData in _roots.Values)
                linkData.ValidateLinksResolved();
        }

        private RootLinkData GetRootLinkData (JsonLinkedContext context)
        {
            var root = GetRoot<TRoot>(context);
            return _roots.GetOrAdd(root, () => new RootLinkData(root));
        }

        protected class RootLinkData : LinkData
        {
            private readonly TRoot _root;

            public RootLinkData (TRoot root)
            {
                _root = root;
            }

            public override void ValidateLinksResolved ()
            {
                if (_unresolvedLinks.Any()) {
                    throw new JsonLinkProviderException("JSON scoped unique link provider for {0} (root={1}) contains unresolved links within root {2}: '{3}'."
                        .Fmt(typeof(TValue).Name, typeof(TRoot).Name, _root, _unresolvedLinks.JoinString("', '")));
                }
            }
        }
    }
}