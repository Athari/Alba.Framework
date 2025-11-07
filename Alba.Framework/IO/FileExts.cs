namespace Alba.Framework.IO;

public static class FileExts
{
    extension(File)
    {
        public static bool TryDelete(string? path)
        {
            try {
                if (File.Exists(path)) {
                    File.Delete(path);
                    return true;
                }
            }
            catch { /* ignored */
            }
            return false;
        }
    }
}