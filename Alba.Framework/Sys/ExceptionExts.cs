using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Runtime.CompilerServices;
using System.Text;
using Alba.Framework.Collections;
using Alba.Framework.Reflection;
using Alba.Framework.Text;
using Microsoft.CSharp.RuntimeBinder;
using TypeLoadException = System.Reflection.ReflectionTypeLoadException;

// ReSharper disable RedundantJumpStatement
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
            var sb = new StringBuilder();
            foreach (Exception e in @this.TraverseList(e => e.InnerException))
                sb.AppendSentence(GetMessageWithSubExceptions(e));
            return sb.ToString().SingleLine();
        }

        private static string GetMessageWithSubExceptions (Exception exc)
        {
            IReadOnlyList<Exception> subExcs = GetSubExceptions(exc);
            if (subExcs == null || !subExcs.Any())
                return exc.Message;

            var sb = new StringBuilder();
            for (int i = 0; i < subExcs.Count; i++)
                sb.AppendSentence("{0}: {1}".Fmt(i + 1, subExcs[i].GetFullMessage()));
            sb.Insert(0, exc.Message + " (");
            sb.Append(")");
            return sb.ToString();
        }

        private static IReadOnlyList<Exception> GetSubExceptions (Exception exc)
        {
            var aggregateException = exc as AggregateException;
            if (aggregateException != null)
                return aggregateException.InnerExceptions;
            var reflectionTypeLoadException = exc as TypeLoadException;
            if (reflectionTypeLoadException != null)
                return reflectionTypeLoadException.LoaderExceptions;
            var smtpFailedRecipientsException = exc as SmtpFailedRecipientsException;
            if (smtpFailedRecipientsException != null)
                return smtpFailedRecipientsException.InnerExceptions;
            var compositionException = exc as CompositionException;
            if (compositionException != null)
                return compositionException.RootCauses;
            return null;
        }

        public static bool IsIOException (this Exception @this)
        {
            return @this is IOException || @this is UnauthorizedAccessException;
        }
    }
}