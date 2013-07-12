using System;
using System.Runtime.Serialization;

namespace Alba.Framework.Serialization.Json
{
    [Serializable]
    public class JsonLinkProviderException : Exception
    {
        public JsonLinkProviderException ()
        {}

        public JsonLinkProviderException (string message) : base(message)
        {}

        public JsonLinkProviderException (string message, Exception inner) : base(message, inner)
        {}

        protected JsonLinkProviderException (SerializationInfo info, StreamingContext context) : base(info, context)
        {}
    }
}