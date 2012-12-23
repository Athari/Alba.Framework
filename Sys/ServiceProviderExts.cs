using System;

namespace Alba.Framework.Sys
{
    public static class ServiceProviderExts
    {
        public static T GetService<T> (this IServiceProvider @this)
        {
            return (T)@this.GetService(typeof(T));
        }
    }
}