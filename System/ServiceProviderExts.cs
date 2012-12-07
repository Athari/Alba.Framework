using System;

namespace Alba.Framework.System
{
    public static class ServiceProviderExts
    {
        public static T GetService<T> (this IServiceProvider @this)
        {
            return (T)@this.GetService(typeof(T));
        }
    }
}