using System;
using System.Collections.Generic;
using Alba.Framework.Common;
using Alba.Framework.Logs;
using Alba.Framework.Text;
using Newtonsoft.Json;

// ReSharper disable StaticFieldInGenericType
namespace Alba.Framework.Serialization.Json
{
    public abstract class JsonUniqueLinkProviderBase<TValue> : JsonLinkProvider<TValue>
        where TValue : IIdentifiable<string>
    {
        private static readonly Lazy<ILog> _log = new Lazy<ILog>(() => new Log<JsonUniqueLinkProviderBase<TValue>>(AlbaFrameworkTraceSources.Serialization));

        protected JsonUniqueLinkProviderBase (string idProp)
            : base(idProp)
        {}

        private static ILog Log
        {
            get { return _log.Value; }
        }

        protected abstract class LinkData
        {
            private readonly IDictionary<string, TValue> _links = new Dictionary<string, TValue>();
            protected readonly ISet<string> _unresolvedLinks = new HashSet<string>();

            public string GetLink (TValue value, JsonSerializer serializer, JsonLinkedContext context)
            {
                string id = value.Id;
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