using System.Runtime.InteropServices;
using Alba.Framework.Win32;

// TODO Implement proper zone check, see http://blogs.msdn.com/b/oldnewthing/archive/2013/11/04/10463035.aspx
namespace Alba.Framework.Security;

public static class UrlZones
{
    public static UrlZone? GetFileZone(string fileName)
    {
        using var zoneId = PersistentZoneIdentifier.Create();
        try {
            zoneId.File.Load(fileName, 0);
            return zoneId.Zone.GetId();
        }
        catch (FileNotFoundException) { // if no MOTW, loading fails
            return null;
        }
    }

    public static void SetFileZone(string fileName, UrlZone urlZone)
    {
        using var zoneId = PersistentZoneIdentifier.Create();
        zoneId.Zone.SetId(urlZone);
        zoneId.File.Save(fileName, false);
    }

    public static void RemoveFileZone(string fileName)
    {
        using var zoneId = PersistentZoneIdentifier.Create();
        try {
            zoneId.Zone.Remove();
            zoneId.File.Save(fileName, false);
        }
        catch (COMException) { } // E_FAIL if no MOTW
    }
}