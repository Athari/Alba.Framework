using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Alba.Framework.Collections;
using Alba.Framework.Common;
using Alba.Framework.Diagnostics;
using Alba.Framework.Text;
using Newtonsoft.Json;

namespace Alba.Framework.Serialization.Json
{
    public class JsonLinkedContext
    {
        public const string TypePropName = "$type";
        public const string IndexPropName = "$index";
        public const string LinkPathSeparator = "/";
        public const char LinkPathSeparatorChar = '/';
        public const char LinkTypeSeparator = ':';

        private static readonly ILog Log = AlbaFrameworkTraceSources.Serialization.GetLog<JsonLinkedContext>();

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
            string linkName = Options.GetLinkProvider(value.GetType()).GetLink(value, this);
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

        public IDisposable RememberLinkScoped (object value)
        {
            return new RememberLinkScope(this, value);
        }

        public void ValidateLinksResolved ()
        {
            foreach (IJsonLinkProvider linkProvider in Options.LinkProviders)
                linkProvider.ValidateLinksResolved();
        }

        public void SetOwner (object o)
        {
            var current = Stack[Stack.Count - 1] as IOwned;
            if (current != null && Stack.Count >= 2)
                current.Owner = Stack[Stack.Count - 2];
        }

        public void PushObject (object o)
        {
            Log.Trace("Push {0}".Fmt(o));
            Stack.Add(o);
        }

        public void PopObject (object o)
        {
            Log.Trace("Pop {0}".Fmt(o));
            if (Stack.Last() != o)
                throw new InvalidOperationException("Invalid object popped from the stack.");
            Stack.RemoveAt(Stack.Count - 1);
        }

        public string StackString
        {
            get { return Stack.Select(o => o.GetType().Name).JoinString(">"); }
        }

        private struct RememberLinkScope : IDisposable
        {
            private readonly JsonLinkedContext _context;
            private readonly object _value;

            public RememberLinkScope (JsonLinkedContext context, object value)
            {
                _context = context;
                _value = value;
                _context.PushObject(_value);
                _context.RememberLink(_value);
            }

            public void Dispose ()
            {
                _context.PopObject(_value);
            }
        }
    }
}