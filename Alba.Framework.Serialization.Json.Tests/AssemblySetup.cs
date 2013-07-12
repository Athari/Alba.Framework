using System.Diagnostics;
using Alba.Framework.Diagnostics;
using Alba.Framework.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Alba.Framework.Serialization.Json.Tests
{
    [TestClass]
    public class AssemblySetup
    {
        [AssemblyInitialize]
        public static void AssemblyInitialize (TestContext context)
        {
            AlbaFrameworkTraceSources.Serialization.Switch.Level = SourceLevels.All;
            AlbaFrameworkTraceSources.Serialization.Listeners.Add(new TraceRedirectTraceListener());
        }
    }
}