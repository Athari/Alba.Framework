using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Alba.Framework.Common;
using Newtonsoft.Json;

namespace Alba.Framework.Serialization.Json
{
    public class JsonLinkedContext
    {
        public const string TypePropName = "$";
        public const string LinkPathSeparator = "/";
        public const char LinkTypeSeparator = ':';

        internal IList<object> Stack { get; private set; }
        public JsonLinkedOptions Options { get; private set; }

        public JsonLinkedContext (JsonLinkedOptions options)
        {
            Stack = new List<object>();
            Options = options;
        }

        public JsonLinkedContext (IEnumerable<IJsonLinkProvider> linkProviders) :
            this(new JsonLinkedOptions())
        {
            Options.LinkProviders.AddRange(linkProviders);
        }

        public static JsonLinkedContext Get (StreamingContext streamingContext)
        {
            return (JsonLinkedContext)streamingContext.Context;
        }

        public object ResolveOrigin (string id, JsonResolveLinkContext resolveContext)
        {
            if (id == null)
                return resolveContext.CreateEmpty("");
            return Options.GetLinkProvider(resolveContext.Type).ResolveOrigin(id, resolveContext);
        }

        public object ResolveLink (string link, JsonResolveLinkContext resolveContext)
        {
            if (link == null)
                throw new ArgumentNullException("link");
            return Options.GetLinkProvider(resolveContext.Type).ResolveLink(link, resolveContext);
        }

        public string GetTypedLink (object value, JsonSerializer serializer)
        {
            if (value == null)
                return null;
            string linkName = Options.GetLinkProvider(value.GetType()).GetLink(value, serializer, this);
            string typeName = serializer.Binder.BindToName(value.GetType());
            return typeName == null ? linkName : typeName + LinkTypeSeparator + linkName;
        }

        public void RememberLink (object value)
        {
            if (value == null)
                throw new ArgumentNullException("value");
            var linkProvider = Options.TryGetLinkProvider(value.GetType());
            if (linkProvider != null)
                linkProvider.RememberOriginLink(value, this);
        }

        public void ValidateLinksResolved ()
        {
            foreach (IJsonLinkProvider linkProvider in Options.LinkProviders)
                linkProvider.ValidateLinksResolved();
        }

        public void SetOwner (object o)
        {
            var current = Stack[Stack.Count - 1] as IOwned;
            if (current != null)
                current.Owner = Stack[Stack.Count - 2];
        }

        public void PushObject (object o)
        {
            Stack.Add(o);
        }

        public void PopObject (object o)
        {
            if (Stack.Last() != o)
                throw new InvalidOperationException("Invalid object popped from the stack.");
            Stack.RemoveAt(Stack.Count - 1);
        }

        public string StackString
        {
            get { return String.Join(">", Stack.Select(o => o.GetType().Name)); }
        }
    }
}