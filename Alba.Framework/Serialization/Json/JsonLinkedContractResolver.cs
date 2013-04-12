using System;
using System.Collections;
using System.Reflection;
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
            if ((property.DefaultValueHandling ?? DefaultValueHandling.Ignore).HasAny(DefaultValueHandling.Ignore)
                && !typeof(string).IsAssignableFrom(property.PropertyType)
                && typeof(IEnumerable).IsAssignableFrom(property.PropertyType)) {
                var memberProp = member as PropertyInfo;
                var memberField = member as FieldInfo;
                Predicate<object> shouldSerialize = obj => {
                    object value = memberProp != null ? memberProp.GetValue(obj) : memberField != null ? memberField.GetValue(obj) : null;
                    var collection = value as ICollection;
                    return collection == null || collection.Count != 0;
                };
                property.ShouldSerialize = property.ShouldSerialize.Merge(shouldSerialize, (a, b) => a && b);
            }
            return property;
        }
    }
}