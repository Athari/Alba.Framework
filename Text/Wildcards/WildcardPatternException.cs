using System;
using System.Runtime.Serialization;
using Alba.Framework.Attributes;

namespace Alba.Framework.Text
{
    [Serializable]
    public class WildcardPatternException : Exception
    {
        public WildcardPatternException ()
        {}

        public WildcardPatternException (string message) : base(message)
        {}

        public WildcardPatternException (string message, Exception innerException) : base(message, innerException)
        {}

        protected WildcardPatternException ([NotNull] SerializationInfo info, StreamingContext context) : base(info, context)
        {}
    }
}