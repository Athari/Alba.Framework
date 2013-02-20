namespace Alba.Framework.Sys
{
    public static class ObjectExts
    {
        public static string GetTypeFullName (this object @this)
        {
            return @this == null ? "null" : @this.GetType().FullName;
        }

        public static T To<T> (this object @this)
        {
            return (T)@this;
        }

        public static string NullableToString (this object @this)
        {
            return @this == null ? "" : @this.ToString();
        }
    }
}