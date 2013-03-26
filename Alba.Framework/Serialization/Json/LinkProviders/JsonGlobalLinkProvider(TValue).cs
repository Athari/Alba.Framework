using System;
using System.Collections.Generic;
using System.Linq;
using Alba.Framework.Common;
using Newtonsoft.Json;

namespace Alba.Framework.Serialization.Json
{
    public class JsonGlobalLinkProvider<TValue> : JsonLinkProvider<TValue>
        where TValue : IIdentifiable<string>
    {
        private readonly IDictionary<string, TValue> _links = new Dictionary<string, TValue>();
        private readonly ISet<string> _unresolvedLinks = new HashSet<string>();

        public JsonGlobalLinkProvider (string idProp)
            : base(idProp)
        {}

        public override string GetLink (TValue value, JsonSerializer serializer, JsonLinkedContext context)
        {
            return value.Id;
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
                throw new JsonException(string.Format(
                    "JSON global link provider for {0} contains unresolved links: '{1}'.",
                    typeof(TValue).Name, string.Join("', '", _unresolvedLinks)));
            }
        }

        private object GetValue (string link, JsonResolveLinkContext resolveContext, bool isOrigin)
        {
            string untypedLink = GetUntypedLink(link);
            TValue value;
            if (!_links.TryGetValue(untypedLink, out value)) {
                Console.WriteLine("  {0} - created  ({1})", link, resolveContext.Context.StackString);
                _links[untypedLink] = value = (TValue)resolveContext.CreateEmpty(link);
                if (!isOrigin)
                    _unresolvedLinks.Add(untypedLink);
            }
            else {
                Console.WriteLine("  {0} - resolved ({1})", link, resolveContext.Context.StackString);
                if (isOrigin)
                    _unresolvedLinks.Remove(untypedLink);
            }
            return value;
        }
    }
}