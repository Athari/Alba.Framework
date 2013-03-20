using System;
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
    }
}