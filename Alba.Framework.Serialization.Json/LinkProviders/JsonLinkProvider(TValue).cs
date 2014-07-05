using System;
using System.Collections.Generic;
using Alba.Framework.Collections;
using Alba.Framework.Common;
using Alba.Framework.Reflection;
using Alba.Framework.Text;

namespace Alba.Framework.Serialization.Json
{
    public abstract class JsonLinkProvider<TValue> : IJsonLinkProvider
    {
        public string IdProp { get; private set; }

        protected JsonLinkProvider (string idProp)
        {
            IdProp = idProp;
        }

        protected bool IsIndexed
        {
            get { return IdProp == JsonLinkedContext.IndexPropName; }
        }

        public virtual bool IsScoped
        {
            get { return false; }
        }

        protected static string GetUntypedLink (string link)
        {
            int typeSeparatorPos = link.IndexOf(JsonLinkedContext.LinkTypeSeparator);
            return typeSeparatorPos == -1 ? link : link.Substring(typeSeparatorPos + 1);
        }

        protected static TRoot GetRoot<TRoot> (JsonLinkedContext context)
            where TRoot : class
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

        protected string GetDebugId (TValue value)
        {
            if (IsIndexed)
                return JsonLinkedContext.IndexPropName;
            var idable = value as IIdentifiable<string>;
            if (idable == null)
                return "?";
            string id = idable.Id;
            if (id == null)
                return "?";
            return id;
        }

        public virtual bool CanLink (Type type)
        {
            return type.Is<TValue>();
        }

        string IJsonLinkProvider.GetLink (object value, JsonLinkedContext context)
        {
            return GetLink((TValue)value, context);
        }

        public abstract string GetLink (TValue value, JsonLinkedContext context);

        public abstract object ResolveOrigin (string id, JsonResolveLinkContext resolveContext);

        public abstract object ResolveLink (string link, JsonResolveLinkContext resolveContext);

        void IJsonLinkProvider.RememberOriginLink (object value, JsonLinkedContext context)
        {
            RememberOriginLink((TValue)value, context);
        }

        public abstract void ValidateLinksResolved ();

        public virtual void RememberOriginLink (TValue value, JsonLinkedContext context)
        {}
    }
}