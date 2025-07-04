using System.Reflection;

namespace Alba.Framework.IO;

[PublicAPI]
public static class Paths
{
    public static string ExecutableFilePath =>
        Assembly.GetExecutingAssembly().Location;

    public static string ExecutableFileDir =>
        Path.GetDirectoryName(ExecutableFilePath) ?? ".";

    public static string GetTempFileName(string ext) =>
        $"{Path.GetTempPath()}{Guid.NewGuid():N}.{ext}";

    public static string GetRoamingAppDir(string company, string product) =>
        GetAppDir(Environment.SpecialFolder.ApplicationData, company, product);

    public static string GetLocalAppDir(string company, string product) =>
        GetAppDir(Environment.SpecialFolder.LocalApplicationData, company, product);

    public static string GetCommonAppDir(string company, string product) =>
        GetAppDir(Environment.SpecialFolder.CommonApplicationData, company, product);

    private static string GetAppDir(Environment.SpecialFolder folder, string company, string product)
    {
        string dir = Path.Combine(folder.GetPath(), company, product);
        Directory.CreateDirectory(dir);
        return dir;
    }

    public static string GetPath(this Environment.SpecialFolder @this,
        Environment.SpecialFolderOption option = Environment.SpecialFolderOption.Create) =>
        Environment.GetFolderPath(@this, option);
}