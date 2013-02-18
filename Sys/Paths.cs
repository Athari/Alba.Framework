using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Alba.Framework.Text;

namespace Alba.Framework.Sys
{
    public static class Paths
    {
        public static string ExecutableFilePath
        {
            get { return Assembly.GetExecutingAssembly().CodeBase; }
        }

        public static string GetTempFileName (string ext)
        {
            return string.Format("{0}{1}.{2}", Path.GetTempPath(), Guid.NewGuid().ToString("N"), ext);
        }

        public static string GetRoamingAppDir (string company, string product)
        {
            return GetAppDir(Environment.SpecialFolder.ApplicationData, company, product);
        }

        public static string GetLocalAppDir (string company, string product)
        {
            return GetAppDir(Environment.SpecialFolder.LocalApplicationData, company, product);
        }

        public static string GetCommonAppDir (string company, string product)
        {
            return GetAppDir(Environment.SpecialFolder.CommonApplicationData, company, product);
        }

        private static string GetAppDir (Environment.SpecialFolder folder, string company, string product)
        {
            string dir = Combine(folder.GetPath(), company, product);
            EnsureDirExists(dir);
            return dir;
        }

        public static void EnsureDirExists (string dir)
        {
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
        }

        public static string Combine (params string[] paths)
        {
            return paths.Where(path => !path.IsNullOrEmpty()).Aggregate(string.Empty, Path.Combine);
        }

        public static string GetPath (this Environment.SpecialFolder @this,
            Environment.SpecialFolderOption option = Environment.SpecialFolderOption.Create)
        {
            return Environment.GetFolderPath(@this, option);
        }
    }
}