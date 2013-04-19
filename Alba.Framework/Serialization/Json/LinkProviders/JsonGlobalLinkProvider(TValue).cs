using System;
using System.Collections.Generic;
using System.Linq;
using Alba.Framework.Collections;
using Alba.Framework.Common;
using Alba.Framework.Logs;
using Alba.Framework.Text;
using Newtonsoft.Json;

// ReSharper disable StaticFieldInGenericType
namespace Alba.Framework.Serialization.Json
{
    public class JsonGlobalLinkProvider<TValue> : JsonLinkProvider<TValue>
        where TValue : IIdentifiable<string>
    {
        private static readonly Lazy<ILog> _log = new Lazy<ILog>(() => new Log<JsonGlobalLinkProvider<TValue>>(AlbaFrameworkTraceSources.Serialization));

        private readonly IDictionary<string, TValue> _links = new Dictionary<string, TValue>();
        private readonly ISet<string> _unresolvedLinks = new HashSet<string>();

        public JsonGlobalLinkProvider (string idProp)
            : base(idProp)
        {}

        private static ILog Log
        {
            get { return _log.Value; }
        }

        public override string GetLink (TValue value, JsonSerializer serializer, JsonLinkedContext context)
        {
            string id = value.Id;
            if (id == null)
                throw new JsonLinkProviderException("Id of origin '{0}' not specified.".Fmt(value));
            return id;
        }

        public override object ResolveOrigin (string id, JsonResolveLinkContext resolveContext)
        {
            return GetValue(id, resolveContext, true);
        }

        public override object ResolveLink (string link, JsonResolveLinkContext resolveContext)
        {
            return GetValue(link, resolveContext, false);
        }

        public override void ValidateLinksResolved ()
        {
            if (_unresolvedLinks.Any()) {
                throw new JsonLinkProviderException("JSON global link provider for {0} contains unresolved links: '{1}'."
                    .Fmt(typeof(TValue).Name, _unresolvedLinks.JoinString("', '")));
            }
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
    }
}