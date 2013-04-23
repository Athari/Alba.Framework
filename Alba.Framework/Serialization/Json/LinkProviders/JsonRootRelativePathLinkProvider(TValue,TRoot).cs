using System;
using System.Collections.Generic;
using System.Linq;
using Alba.Framework.Collections;
using Alba.Framework.Common;
using Alba.Framework.Text;
using Newtonsoft.Json;

// ReSharper disable StaticFieldInGenericType
namespace Alba.Framework.Serialization.Json
{
    public class JsonRootRelativePathLinkProvider<TValue, TRoot> : JsonPathLinkProviderBase<TValue, TRoot>
        where TValue : class, IIdentifiable<string>
        where TRoot : class
    {
        private readonly IDictionary<TRoot, RootLinkData> _roots = new Dictionary<TRoot, RootLinkData>();

        public JsonRootRelativePathLinkProvider (string idProp) :
            base(idProp)
        {}

        public override string GetLink (TValue value, JsonSerializer serializer, JsonLinkedContext context)
        {
            return GetRelativeLink(GetRootLinkData(context).GetLink(value), GenerateLink(context, true) ?? "");
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
            throw new JsonLinkProviderException("Root not found. (IOwner interface missing? Default contructor missing?) Stack contents: {0}."
                .Fmt(context.Stack.JoinString("; ")));
        }

        private static string GetRelativeLink (string linkTo, string linkFrom)
        {
            if (linkFrom == "")
                return linkTo;
            if (linkTo == null)
                return null;
            /*if (linkTo.StartsWith(linkFrom + "/"))
                return linkTo.Sub(linkFrom.Length + 1);*/

            string[] partsTo = linkTo.Split(JsonLinkedContext.LinkPathSeparatorChar);
            string[] partsFrom = linkFrom.Split(JsonLinkedContext.LinkPathSeparatorChar);
            int iCommon, maxCommon = Math.Min(partsTo.Length, partsFrom.Length);
            for (iCommon = 0; iCommon < maxCommon && partsTo[iCommon] == partsFrom[iCommon]; iCommon++) {}
            return new string(JsonLinkedContext.LinkPathSeparatorChar, partsFrom.Length - iCommon) + 
                partsTo.TakeLast(partsTo.Length - iCommon).JoinString(JsonLinkedContext.LinkPathSeparator);
        }

        protected class RootLinkData : LinkData
        {
            private readonly TRoot _root;

            public RootLinkData (TRoot root)
            {
                _root = root;
            }

            public override void ValidateLinksResolved ()
            {
                if (_unresolvedLinks.Any()) {
                    throw new JsonLinkProviderException("JSON path link provider for {0} (root={1}) contains unresolved links within root {2}: '{3}'."
                        .Fmt(typeof(TValue).Name, typeof(TRoot).Name, _root, _unresolvedLinks.JoinString("', '")));
                }
            }
        }
    }
}