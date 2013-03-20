using System;
using System.Collections.Generic;
using System.Linq;

namespace Alba.Framework.Serialization.Json
{
    public class JsonLinkedOptions
    {
        public List<IJsonLinkProvider> LinkProviders { get; private set; }

        public JsonLinkedOptions ()
        {
            LinkProviders = new List<IJsonLinkProvider>();
        }

        public IJsonLinkProvider TryGetLinkProvider (Type type)
        {
            return LinkProviders.FirstOrDefault(lp => lp.CanLink(type));
        }

        public IJsonLinkProvider GetLinkProvider (Type type)
        {
            return LinkProviders.First(lp => lp.CanLink(type));
        }
    }
}