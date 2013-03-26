using System;
using System.Collections.Generic;
using System.Linq;
using Alba.Framework.Collections;
using Alba.Framework.Common;
using Newtonsoft.Json;

namespace Alba.Framework.Serialization.Json
{
    public class JsonPathLinkProvider<TValue, TRoot> : JsonLinkProvider<TValue>
        where TValue : class, IIdentifiable<string>
        where TRoot : class
    {
        private readonly IDictionary<TRoot, RootLinkData> _roots = new Dictionary<TRoot, RootLinkData>();

        public JsonPathLinkProvider (string idProp) :
            base(idProp)
        {}

        public override string GetLink (TValue value, JsonSerializer serializer, JsonLinkedContext context)
        {
            return GetRootLinkData(context).GetLink(value);
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

        public override void RememberOriginLink (TValue value, JsonLinkedContext context)
        {
            GetRootLinkData(context).RememberOriginLink(value, context);
        }

        private RootLinkData GetRootLinkData (JsonLinkedContext context)
        {
            TRoot root = GetRoot(context);
            return _roots.GetOrAdd(root, () => new RootLinkData(root));
        }

        private static TRoot GetRoot (JsonLinkedContext context)
        {
            IList<object> stack = context.Stack;
            for (int i = stack.Count - 1; i >= 0; i--) {
                var root = stack[i] as TRoot;
                if (root != null)
                    return root;
            }
            throw new InvalidOperationException("Root not found.");
        }

        private static string GenerateLink (JsonLinkedContext context)
        {
            var path = new List<string>();
            IList<object> stack = context.Stack;
            for (int i = stack.Count - 1; i >= 0 && !(stack[i] is TRoot); i--) {
                var idable = stack[i] as IIdentifiable<string>;
                if (idable != null)
                    path.Add(idable.Id);
            }
            path.Reverse();
            return string.Join(JsonLinkedContext.LinkPathSeparator, path);
        }

        private class RootLinkData
        {
            private readonly TRoot _root;
            private readonly IDictionary<string, TValue> _pathToValue = new Dictionary<string, TValue>();
            private readonly IDictionary<TValue, string> _valueToPath = new Dictionary<TValue, string>();
            private readonly ISet<string> _unresolvedLinks = new HashSet<string>();

            public RootLinkData (TRoot root)
            {
                _root = root;
            }

            public string GetLink (TValue value)
            {
                if (value == null)
                    throw new ArgumentNullException("value");
                try {
                    return _valueToPath[value];
                }
                catch (KeyNotFoundException e) {
                    throw new KeyNotFoundException(string.Format("Object '{0}' of type '{1}' (id={2}) is not a valid link as it is not contained within the root object.",
                        value, value.GetType(), value.Id), e);
                }
            }

            public TValue ResolveOrigin (string id, JsonResolveLinkContext resolveContext)
            {
                string untypedId = GetUntypedLink(id);
                string path = GenerateLink(resolveContext.Context) + JsonLinkedContext.LinkPathSeparator + untypedId;
                TValue value;
                if (!_pathToValue.TryGetValue(path, out value)) {
                    Console.WriteLine("  {0} - created origin in {1}", path, resolveContext.Context.StackString);
                    value = (TValue)resolveContext.CreateEmpty(id);
                    _pathToValue[path] = value;
                    _valueToPath[value] = path;
                }
                else {
                    _unresolvedLinks.Remove(path);
                    Console.WriteLine("  {0} - resolved origin in {1}", path, resolveContext.Context.StackString);
                }
                return value;
            }

            public TValue ResolveLink (string path, JsonResolveLinkContext resolveContext)
            {
                string untypedPath = GetUntypedLink(path);
                TValue value;
                if (!_pathToValue.TryGetValue(untypedPath, out value)) {
                    Console.WriteLine("  {0} - created link in {1}", path, resolveContext.Context.StackString);
                    value = (TValue)resolveContext.CreateEmpty(path);
                    _pathToValue[untypedPath] = value;
                    _valueToPath[value] = untypedPath;
                    _unresolvedLinks.Add(untypedPath);
                }
                else {
                    Console.WriteLine("  {0} - resolved link in {1}", path, resolveContext.Context.StackString);
                }
                return value;
            }

            public void RememberOriginLink (TValue value, JsonLinkedContext context)
            {
                if (value == null)
                    throw new ArgumentNullException("value");
                string path = GenerateLink(context);
                try {
                    _pathToValue.Add(path, value);
                    _valueToPath.Add(value, path);
                }
                catch (ArgumentException e) {
                    throw new ArgumentException(string.Format("Duplicate value '{0}' (id={1})", value, value.Id), e);
                }
            }

            public void ValidateLinksResolved ()
            {
                if (_unresolvedLinks.Any()) {
                    throw new JsonException(string.Format(
                        "JSON path link provider for {0} (root={1}) contains unresolved links within root {2}: '{3}'.",
                        typeof(TValue).Name, typeof(TRoot).Name, _root, string.Join("', '", _unresolvedLinks)));
                }
            }
        }
    }
}