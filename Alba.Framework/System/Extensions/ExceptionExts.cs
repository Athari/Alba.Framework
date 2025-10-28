using System.Diagnostics.Contracts;
using System.Net.Mail;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Text;
using Alba.Framework.Collections;
using Alba.Framework.Reflection;
using Alba.Framework.Text;
using Microsoft.CSharp.RuntimeBinder;
using Binder = Microsoft.CSharp.RuntimeBinder.Binder;
using TypeLoadException = System.Reflection.ReflectionTypeLoadException;

namespace Alba.Framework;

using CreateFromStrInnerExcSiteType = CallSite<Func<CallSite, Type, string, Exception, Exception>>;
using CreateFromStrSiteType = CallSite<Func<CallSite, Type, string, Exception>>;

[PublicAPI]
public static class ExceptionExts
{
    private static readonly CSharpArgumentInfo ArgInfoNone = CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null);
    private static readonly CSharpArgumentInfo ArgInfoStatic = CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null);
    private static readonly Lazy<CreateFromStrSiteType> CreateFromStrSite = new(() =>
        CreateFromStrSiteType.Create(Binder.InvokeConstructor(CSharpBinderFlags.None, null, [ ArgInfoStatic, ArgInfoNone ])));
    private static readonly Lazy<CreateFromStrInnerExcSiteType> CreateFromStrAndInnerExcSite = new(() =>
        CreateFromStrInnerExcSiteType.Create(Binder.InvokeConstructor(CSharpBinderFlags.None, null, [ ArgInfoStatic, ArgInfoNone, ArgInfoNone ])));

    public static Exception Create(Type exceptionType, string message)
    {
        Contract.Requires<ArgumentException>(exceptionType.Is<Exception>(), "exceptionType");
        return CreateFromStrSite.Value.Target(CreateFromStrSite.Value, exceptionType, message);
    }

    public static Exception Create(Type exceptionType, string message, Exception innerException)
    {
        Contract.Requires<ArgumentException>(exceptionType.Is<Exception>(), "exceptionType");
        return CreateFromStrAndInnerExcSite.Value.Target(CreateFromStrAndInnerExcSite.Value, exceptionType, message, innerException);
    }

    public static TException Create<TException>(string message) where TException : Exception
    {
        return (TException)CreateFromStrSite.Value.Target(CreateFromStrSite.Value, typeof(TException), message);
    }

    public static TException Create<TException>(string message, Exception innerException) where TException : Exception
    {
        return (TException)CreateFromStrAndInnerExcSite.Value.Target(CreateFromStrAndInnerExcSite.Value, typeof(TException), message, innerException);
    }

    public static string GetFullMessage(this Exception @this)
    {
        var sb = new StringBuilder();
        foreach (Exception e in @this.TraverseList(e => e.InnerException).Where(IsExceptionMessageIncluded))
            sb.AppendSentence(GetMessageWithSubExceptions(e));
        return sb.ToString().SingleLine();
    }

    private static bool IsExceptionMessageIncluded(Exception e)
    {
        if (e is TargetInvocationException)
            return false;
        return true;
    }

    private static string GetMessageWithSubExceptions(Exception exc)
    {
        IReadOnlyList<Exception>? subExcs = GetSubExceptions(exc);
        if (subExcs == null || !subExcs.Any())
            return exc.Message;

        var sb = new StringBuilder();
        for (int i = 0; i < subExcs.Count; i++)
            sb.AppendSentence($"{i + 1}: {subExcs[i].GetFullMessage()}");
        sb.Insert(0, exc.Message + " (");
        sb.Append(')');
        return sb.ToString();
    }

    private static IReadOnlyList<Exception>? GetSubExceptions(Exception exc)
    {
        return exc switch {
            AggregateException a => a.InnerExceptions,
            TypeLoadException b => [.. b.LoaderExceptions.WhereNotNull()],
            SmtpFailedRecipientsException c => c.InnerExceptions,
            _ => null,
        };
        //var compositionException = exc as CompositionException;
        //if (compositionException != null)
        //    return compositionException.RootCauses;
    }

    public static bool IsIOException(this Exception @this) =>
        @this is IOException or UnauthorizedAccessException;

    [DoesNotReturn, ContractAnnotation("=> halt")]
    public static void Rethrow(this Exception @this) =>
        ExceptionDispatchInfo.Capture(@this).Throw();
}