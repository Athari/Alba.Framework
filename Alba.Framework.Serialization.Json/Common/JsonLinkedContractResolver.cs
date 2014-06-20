using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Alba.Framework.Collections;
using Alba.Framework.Reflection;
using Alba.Framework.Sys;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Alba.Framework.Serialization.Json
{
    public class JsonLinkedContractResolver : DefaultContractResolver
    {
        public JsonLinkedContractResolver (bool shareCache = false)
            : base(shareCache)
        {}

        protected override JsonObjectContract CreateObjectContract (Type objectType)
        {
            var contract = base.CreateObjectContract(objectType);
            contract.OnDeserializingCallbacks.Add((o, ctx) => JsonLinkedContext.Get(ctx).PushObject(o));
            contract.OnDeserializingCallbacks.Add((o, ctx) => JsonLinkedContext.Get(ctx).SetOwner(o));
            contract.OnDeserializedCallbacks.Add((o, ctx) => JsonLinkedContext.Get(ctx).PopObject(o));
            contract.OnSerializingCallbacks.Add((o, ctx) => JsonLinkedContext.Get(ctx).PushObject(o));
            contract.OnSerializedCallbacks.Add((o, ctx) => JsonLinkedContext.Get(ctx).PopObject(o));
            return contract;
        }

        protected override JsonProperty CreateProperty (MemberInfo member, MemberSerialization memberSerialization)
        {
            JsonProperty property = base.CreateProperty(member, memberSerialization);

            // Skip serialization of empty collections.
            if ((property.DefaultValueHandling ?? DefaultValueHandling.Ignore).Has(DefaultValueHandling.Ignore) && IsPropertyCollection(property)) {
                Predicate<object> shouldSerialize = obj => {
                    var collection = property.ValueProvider.GetValue(obj) as ICollection;
                    return collection == null || collection.Count != 0;
                };
                property.ShouldSerialize = property.ShouldSerialize.Merge(shouldSerialize, (a, b) => a && b);
            }

            return property;
        }

        protected override IList<JsonProperty> CreateProperties (Type type, MemberSerialization serialization)
        {
            var properties = new JsonPropertyCollection(type);
            foreach (JsonProperty property in GetSerializableMembers(type).Select(m => CreateProperty(m, serialization)).WhereNotNull())
                properties.AddProperty(property);
            return properties
                .OrderBy(p => p.Order ?? -1)
                .ThenBy(IsPropertyCollection)
                .ThenBy(GetPropertyTypeDepth)
                .ThenBy(p => p.PropertyName)
                .ToList();
        }

        private static int GetPropertyTypeDepth (JsonProperty property)
        {
            // Properties from base classes come before properties from descendant classes.
            return property.DeclaringType.TraverseList(t => t.BaseType).Count();
        }

        private static bool IsPropertyCollection (JsonProperty property)
        {
            // Very simplified. See DefaultContractResolver.CreateContract for proper implementation of collection check.
            return !property.PropertyType.Is<string>() && property.PropertyType.Is<IEnumerable>();
        }
    }
}