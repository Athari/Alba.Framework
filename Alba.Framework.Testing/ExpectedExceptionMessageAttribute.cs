using System;
using System.Text.RegularExpressions;
using Alba.Framework.Reflection;
using Alba.Framework.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Alba.Framework.Testing
{
    [AttributeUsage (AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class ExpectedExceptionMessageAttribute : ExpectedExceptionBaseAttribute
    {
        private static readonly Type TypeNone = typeof(int);

        public Type Type { get; set; }
        public Type InnerType { get; set; }
        public string Message { get; set; }
        public string InnerMessage { get; set; }
        public string NoErrorMessage { get; set; }
        public bool AllowDerivedTypes { get; set; }
        public ExceptionMatch MatchMode { get; set; }

        public ExpectedExceptionMessageAttribute (Type type, string message = null)
        {
            Type = type;
            Message = message;
            InnerType = TypeNone;
        }

        protected override string NoExceptionMessage
        {
            get { return NoErrorMessage ?? base.NoExceptionMessage; }
        }

        protected override void Verify (Exception ex)
        {
            RethrowIfAssertException(ex);
            Assert.IsNotNull(ex);

            AssertException(Type, ex.GetType(), Message, ex.Message, "Exception");

            if (InnerType == TypeNone)
                return;

            Exception ex2 = ex.InnerException;
            if (InnerType == null)
                Assert.IsNull(ex2, ex2 == null ? "" : "Inner exception is of type {0} (message=\"{1}\"), expected null."
                    .Fmt(ex2.GetType().GetFullName(), ex2.Message));
            else {
                Assert.IsNotNull(ex2, "Inner exception is null, expected not null.");
                AssertException(InnerType, ex2.GetType(), InnerMessage, ex2.Message, "Inner exception");
            }
        }

        private void AssertException (Type expectedType, Type actualType, string expectedMessage, string actualMessage,
            string exceptionType)
        {
            Assert.IsTrue(AllowDerivedTypes ? actualType.IsAssignableTo(expectedType) : expectedType == actualType,
                "{0} is of type {1} (message=\"{2}\"), expected {3}."
                    .Fmt(exceptionType, actualType.GetFullName(), actualMessage, expectedType.GetFullName()));

            if (expectedMessage == null)
                return;
            string msg = "{0}'s message \"{1}\" (type={2}) did not match \"{3}\"."
                .Fmt(exceptionType, actualMessage, actualType.GetFullName(), expectedMessage);
            switch (MatchMode) {
                case ExceptionMatch.Exact:
                    Assert.IsTrue(actualMessage.Equals(expectedMessage, StringComparison.CurrentCultureIgnoreCase), msg);
                    break;
                case ExceptionMatch.Substring:
                    Assert.IsTrue(actualMessage.Contains(expectedMessage, StringComparison.CurrentCultureIgnoreCase), msg);
                    break;
                case ExceptionMatch.Regex:
                    Assert.IsTrue(Regex.IsMatch(actualMessage, expectedMessage, RegexOptions.IgnoreCase), msg);
                    break;
                case ExceptionMatch.Wildcard:
                    Assert.IsTrue(WildcardPattern.IsMatch(actualMessage, expectedMessage, WildcardOptions.IgnoreCase), msg);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}