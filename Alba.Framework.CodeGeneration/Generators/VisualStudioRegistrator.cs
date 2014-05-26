using System;
using System.Linq;
using Microsoft.Win32;

namespace Alba.Framework.CodeGeneration.Generators
{
    internal class VisualStudioRegistrator
    {
        private static readonly Guid CSharpGuid = new Guid("FAE04EC1-301F-11D3-BF4B-00C04F79EFBC");

        public static void RegisterGenerator (Type t)
        {
            var attr = GetAttribute<CustomToolAttribute>(t);
            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(GetToolsKeyName(attr.Name, attr.VisualStudioVersion))) {
                key.SetValue("", attr.Description);
                key.SetValue("CLSID", t.GUID.ToString("B"));
                key.SetValue("GeneratesDesignTimeSource", 1);
            }
            //System.Activator.CreateInstance(Type.GetTypeFromCLSID(new Guid("{21fc238d-6f2a-449b-9531-9552a24a75a4}")));
        }

        public static void UnregisterGenerator (Type t)
        {
            var attr = GetAttribute<CustomToolAttribute>(t);
            Registry.LocalMachine.DeleteSubKey(GetToolsKeyName(attr.Name, attr.VisualStudioVersion), false);
        }

        private static TAttr GetAttribute<TAttr> (Type t) where TAttr : Attribute
        {
            return (TAttr)t.GetCustomAttributes(typeof(TAttr), false).SingleOrDefault();
        }

        private static string GetToolsKeyName (string toolName, string visualStudioVersion)
        {
            return string.Format(@"Software\Microsoft\VisualStudio\{0}\Generators\{1:B}\{2}\",
                visualStudioVersion, CSharpGuid, toolName);
        }
    }
}