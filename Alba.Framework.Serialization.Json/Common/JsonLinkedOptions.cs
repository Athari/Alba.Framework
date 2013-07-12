using System;
using System.Collections.Generic;
using System.Linq;
using Alba.Framework.Text;

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
            try {
                return LinkProviders.First(lp => lp.CanLink(type));
            }
            catch (InvalidOperationException e) {
                throw new InvalidOperationException("Link provider for '{0}' not found.".Fmt(type.FullName), e);
            }
        }
    }
}