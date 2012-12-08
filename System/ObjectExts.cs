namespace Alba.Framework.System
{
    public static class ObjectExts
    {
        public static string GetTypeFullName (this object @this)
        {
            return @this == null ? "null" : @this.GetType().FullName;
        }
    }
}