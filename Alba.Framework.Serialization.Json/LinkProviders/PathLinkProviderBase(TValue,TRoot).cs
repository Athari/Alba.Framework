using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Alba.Framework.Collections;
using Alba.Framework.Common;
using Alba.Framework.Diagnostics;
using Alba.Framework.Globalization;
using Alba.Framework.Text;

// ReSharper disable StaticFieldInGenericType
namespace Alba.Framework.Serialization.Json
{
    public abstract class PathLinkProviderBase<TValue, TRoot> : JsonLinkProvider<TValue>
        where TValue : class
        where TRoot : class
    {
        private static readonly ILog Log = AlbaFrameworkTraceSources.Serialization.GetLog<PathLinkProviderBase<TValue, TRoot>>();

        protected PathLinkProviderBase (string idProp) : base(idProp)
        {}

        protected string GenerateLink (JsonLinkedContext context, bool skipTop = false)
        {
            var path = new List<string>();
            IList<object> stack = context.Stack;
            int i;
            for (i = stack.Count - 1; i >= 0; i--) {
                if (stack[i] is TRoot) {
                    if (i == stack.Count - 1)
                        return "";
                    break;
                }
                string id = GetIdAt(stack, i);
                if (id != null)
                    path.Add(id);
            }
            if (i == -1)
                //throw new JsonLinkProviderException("Root of type '{0}' not found.".Fmt(typeof(TRoot).Name));
                return "";
            path.Reverse();
            if (skipTop && path.Count >= 1)
                path.RemoveAt(path.Count - 1);
            return path.Any() ? path.JoinString(JsonLinkedContext.LinkPathSeparator) : null;
        }

        private string GetIdAt (IList<object> stack, int i)
        {
            if (IsIndexed) {
                if (i == stack.Count - 1)
                    return null;
                var collection = stack[i] as IList;
                if (collection == null)
                    return null;
                int index = collection.IndexOf(stack[i + 1]);
                // Use index when serializing, count when deserializing (collection is being added to).
                return (index != -1 ? index : collection.Count).ToStringInv();
            }
            else {
                var idable = stack[i] as IIdentifiable<string>;
                if (idable == null)
                    return null;
                string id = idable.Id;
                if (id == null)
                    return null;
                return id;
            }
        }

        protected abstract class LinkData
        {
            private readonly PathLinkProviderBase<TValue, TRoot> _linkProvider;
            private readonly IDictionary<string, TValue> _pathToValue = new Dictionary<string, TValue>();
            private readonly IDictionary<TValue, string> _valueToPath = new Dictionary<TValue, string>();
            protected readonly ISet<string> _unresolvedLinks = new HashSet<string>();

            protected LinkData (PathLinkProviderBase<TValue, TRoot> linkProvider)
            {
                _linkProvider = linkProvider;
            }

            public string GetLink (TValue value)
            {
                if (value == null)
                    throw new ArgumentNullException("value");
                try {
                    return _valueToPath[value];
                }
                catch (KeyNotFoundException e) {
                    throw new JsonLinkProviderException("Object '{0}' of type '{1}' (id={2}) is not a valid link as it is not contained within the root object."
                        .Fmt(value, value.GetType(), _linkProvider.GetDebugId(value)), e);
                }
            }

            public TValue ResolveOrigin (string id, JsonResolveLinkContext resolveContext)
            {
                string untypedId = GetUntypedLink(id);
                string path = _linkProvider.GenerateLink(resolveContext.Context);
                path = !path.IsNullOrEmpty() ? path + JsonLinkedContext.LinkPathSeparator + untypedId : untypedId;
                TValue value;
                if (!_pathToValue.TryGetValue(path, out value)) {
                    Log.Trace("  {0} - created origin in {1}".Fmt(path, resolveContext.Context.StackString));
                    value = (TValue)resolveContext.CreateEmpty(id);
                    _pathToValue[path] = value;
                    _valueToPath[value] = path;
                }
                else {
                    _unresolvedLinks.Remove(path);
                    Log.Trace("  {0} - resolved origin in {1}".Fmt(path, resolveContext.Context.StackString));
                }
                return value;
            }

            public TValue ResolveLink (string path, JsonResolveLinkContext resolveContext)
            {
                string untypedPath = GetUntypedLink(path);
                TValue value;
                if (!_pathToValue.TryGetValue(untypedPath, out value)) {
                    Log.Trace("  {0} - created link in {1}".Fmt(path, resolveContext.Context.StackString));
                    value = (TValue)resolveContext.CreateEmpty(path);
                    _pathToValue[untypedPath] = value;
                    _valueToPath[value] = untypedPath;
                    _unresolvedLinks.Add(untypedPath);
                }
                else {
                    Log.Trace("  {0} - resolved link in {1}".Fmt(path, resolveContext.Context.StackString));
                }
                return value;
            }

            public void RememberOriginLink (TValue value, JsonLinkedContext context)
            {
                if (value == null)
                    throw new ArgumentNullException("value");
                if (!_linkProvider.IsIndexed) {
                    var idable = value as IIdentifiable<string>;
                    if (idable != null && idable.Id == null)
                        return;
                }
                string path = _linkProvider.GenerateLink(context);
                if (path == null)
                    return;
                try {
                    _pathToValue.Add(path, value);
                }
                catch (ArgumentException e) {
                    throw new JsonLinkProviderException("Duplicate origin path '{0}' (value={1}, id={2})."
                        .Fmt(path, value, _linkProvider.GetDebugId(value)), e);
                }
                try {
                    _valueToPath.Add(value, path);
                }
                catch (ArgumentException e) {
                    throw new JsonLinkProviderException("Duplicate origin value '{0}' (id={1}, path={2})."
                        .Fmt(value, _linkProvider.GetDebugId(value), path), e);
                }
            }

            public abstract void ValidateLinksResolved ();
        }
    }
}