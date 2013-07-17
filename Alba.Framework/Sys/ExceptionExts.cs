using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using Alba.Framework.Reflection;
using Alba.Framework.Text;
using Microsoft.CSharp.RuntimeBinder;

namespace Alba.Framework.Sys
{
    using CreateFromStrSiteType = CallSite<Func<CallSite, Type, string, Exception>>;
    using CreateFromStrInnerExcSiteType = CallSite<Func<CallSite, Type, string, Exception, Exception>>;

    public static class ExceptionExts
    {
        private static readonly CSharpArgumentInfo ArgInfoNone = CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null);
        private static readonly CSharpArgumentInfo ArgInfoStatic = CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null);
        private static readonly Lazy<CreateFromStrSiteType> CreateFromStrSite = new Lazy<CreateFromStrSiteType>(() =>
            CreateFromStrSiteType.Create(Binder.InvokeConstructor(CSharpBinderFlags.None, null, new[] { ArgInfoStatic, ArgInfoNone })));
        private static readonly Lazy<CreateFromStrInnerExcSiteType> CreateFromStrAndInnerExcSite = new Lazy<CreateFromStrInnerExcSiteType>(() =>
            CreateFromStrInnerExcSiteType.Create(Binder.InvokeConstructor(CSharpBinderFlags.None, null, new[] { ArgInfoStatic, ArgInfoNone, ArgInfoNone })));

        public static Exception Create (Type exceptionType, string message)
        {
            Contract.Requires<ArgumentException>(exceptionType.Is<Exception>(), "exceptionType");
            return CreateFromStrSite.Value.Target(CreateFromStrSite.Value, exceptionType, message);
        }

        public static Exception Create (Type exceptionType, string message, Exception innerException)
        {
            Contract.Requires<ArgumentException>(exceptionType.Is<Exception>(), "exceptionType");
            return CreateFromStrAndInnerExcSite.Value.Target(CreateFromStrAndInnerExcSite.Value, exceptionType, message, innerException);
        }

        public static TException Create<TException> (string message) where TException : Exception
        {
            return (TException)CreateFromStrSite.Value.Target(CreateFromStrSite.Value, typeof(TException), message);
        }

        public static TException Create<TException> (string message, Exception innerException) where TException : Exception
        {
            return (TException)CreateFromStrAndInnerExcSite.Value.Target(CreateFromStrAndInnerExcSite.Value, typeof(TException), message, innerException);
        }

        public static string GetFullMessage (this Exception @this)
        {
            var sb = new StringBuilder(@this.Message);
            for (Exception e = @this.InnerException; e != null; e = e.InnerException)
                sb.AppendSentence(e.Message);
            return sb.ToString().SingleLine();
        }

        public static bool IsIOException (this Exception @this)
        {
            return @this is IOException || @this is UnauthorizedAccessException;
        }
    }
}