using System.Collections.Generic;
using Alba.Framework.Common;
using Alba.Framework.Diagnostics;
using Alba.Framework.Text;

// ReSharper disable StaticFieldInGenericType
namespace Alba.Framework.Serialization.Json
{
    public abstract class UniqueLinkProviderBase<TValue> : JsonLinkProvider<TValue>
    {
        private static readonly ILog Log = AlbaFrameworkTraceSources.Serialization.GetLog<UniqueLinkProviderBase<TValue>>();

        protected UniqueLinkProviderBase (string idProp)
            : base(idProp)
        {}

        protected abstract class LinkData
        {
            private readonly IDictionary<string, TValue> _links = new Dictionary<string, TValue>();
            protected readonly ISet<string> _unresolvedLinks = new HashSet<string>();

            public string GetLink (TValue value, JsonLinkedContext context)
            {
                // TODO Support indexed identifiers in UniqueLinkProvider.
                var idable = value as IIdentifiable<string>;
                if (idable == null)
                    throw new JsonLinkProviderException("Id of origin '{0}' not specified.".Fmt(value));
                string id = idable.Id;
                if (id == null)
                    throw new JsonLinkProviderException("Id of origin '{0}' not specified.".Fmt(value));
                return id;
            }

            public object ResolveOrigin (string id, JsonResolveLinkContext resolveContext)
            {
                return GetValue(id, resolveContext, true);
            }

            public object ResolveLink (string link, JsonResolveLinkContext resolveContext)
            {
                return GetValue(link, resolveContext, false);
            }

            private object GetValue (string link, JsonResolveLinkContext resolveContext, bool isOrigin)
            {
                string untypedLink = GetUntypedLink(link);
                TValue value;
                if (!_links.TryGetValue(untypedLink, out value)) {
                    Log.Trace("  {0} - created  ({1})".Fmt(link, resolveContext.Context.StackString));
                    _links[untypedLink] = value = (TValue)resolveContext.CreateEmpty(link);
                    if (!isOrigin)
                        _unresolvedLinks.Add(untypedLink);
                }
                else {
                    Log.Trace("  {0} - resolved ({1})".Fmt(link, resolveContext.Context.StackString));
                    if (isOrigin)
                        _unresolvedLinks.Remove(untypedLink);
                }
                return value;
            }

            public abstract void ValidateLinksResolved ();
        }
    }
}