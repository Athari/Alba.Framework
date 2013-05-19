using System;
using System.Runtime.InteropServices;
using System.Linq;
using Microsoft.Win32;

namespace Alba.Framework.CodeGeneration.Generators
{
    public class Registrator
    {
        private static readonly Guid CSharpGuid = new Guid("FAE04EC1-301F-11D3-BF4B-00C04F79EFBC");
        private const string VisualStudioVersion = "11.0";

        internal static void RegisterGenerator (Type t)
        {
            var guidAttr = GetAttribute<GuidAttribute>(t);
            var toolAttr = GetAttribute<CustomToolAttribute>(t);
            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(GetToolsKeyName(toolAttr.Name))) {
                key.SetValue("", toolAttr.Description);
                key.SetValue("CLSID", "{" + guidAttr.Value + "}");
                key.SetValue("GeneratesDesignTimeSource", 1);
            }
        }

        internal static void UnregisterGenerator (Type t)
        {
            var toolAttr = GetAttribute<CustomToolAttribute>(t);
            Registry.LocalMachine.DeleteSubKey(GetToolsKeyName(toolAttr.Name), false);
        }

        private static TAttr GetAttribute<TAttr> (Type t) where TAttr : Attribute
        {
            return (TAttr)t.GetCustomAttributes(typeof(TAttr), false).SingleOrDefault();
        }

        private static string GetToolsKeyName (string toolName)
        {
            return string.Format(@"Software\Microsoft\VisualStudio\{0}\Generators\{{{1}}}\{2}\",
                VisualStudioVersion, CSharpGuid, toolName);
        }
    }
}