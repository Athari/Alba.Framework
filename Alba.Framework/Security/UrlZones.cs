using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

// TODO Implement proper zone check, see http://blogs.msdn.com/b/oldnewthing/archive/2013/11/04/10463035.aspx
namespace Alba.Framework.Security
{
    public static class UrlZones
    {
        public static UrlZone GetUrlZone (string fileName)
        {
            var persistentZoneId = new PersistentZoneIdentifier();
            try {
                var zoneId = (IZoneIdentifier)persistentZoneId;
                var persistFile = (IPersistFile)persistentZoneId;
                try {
                    persistFile.Load(fileName, 0);
                    return zoneId.GetId();
                }
                catch (FileNotFoundException) { // if no MOTW, loading fails
                    return UrlZone.LocalMachine;
                }
            }
            finally {
                Marshal.ReleaseComObject(persistentZoneId);
            }
        }

        public static void SetUrlZone (string fileName, UrlZone urlZone)
        {
            var persistentZoneId = new PersistentZoneIdentifier();
            try {
                var zoneId = (IZoneIdentifier)persistentZoneId;
                var persistFile = (IPersistFile)persistentZoneId;
                zoneId.SetId(urlZone);
                persistFile.Save(fileName, false);
            }
            finally {
                Marshal.ReleaseComObject(persistentZoneId);
            }
        }

        public static void RemoveUrlZone (string fileName)
        {
            var persistentZoneId = new PersistentZoneIdentifier();
            try {
                var zoneId = (IZoneIdentifier)persistentZoneId;
                var persistFile = (IPersistFile)persistentZoneId;
                try {
                    zoneId.Remove();
                    persistFile.Save(fileName, false);
                }
                catch (COMException) {} // E_FAIL if no MOTW
            }
            finally {
                Marshal.ReleaseComObject(persistentZoneId);
            }
        }
    }
}