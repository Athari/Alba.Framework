using Alba.Framework.IO;
using Avalonia.Platform.Storage;

namespace Alba.Framework.Avalonia.Storage;

[PublicAPI]
public static class StorageExts
{
    public static string GetPath(this IStorageItem @this) =>
        OperatingSystem.IsAndroid() ? @this.Path.AbsoluteUri : @this.Path.LocalPath;

    public static string GetExt(this IStorageItem @this) =>
        Paths.GetExt(@this.Name);

    public static string GetNameWithoutExt(this IStorageItem @this) =>
        Paths.GetNameWithoutExt(@this.Name);

    public static string ChangeName(this IStorageItem @this,
        Func<string, string?>? name = null,
        Func<string, string?>? ext = null)
    {
        var itemName = @this.GetNameWithoutExt();
        var itemExt = @this.GetExt();
        return $"{name?.Invoke(itemName) ?? itemName}{ext?.Invoke(itemExt) ?? itemExt}";
    }

    public static async IAsyncEnumerable<IStorageFile> GetAllFilesAsync(this IStorageFolder @this)
    {
        await foreach (var item in @this.GetItemsAsync()) {
            if (item is IStorageFile file)
                yield return file;
            if (item is IStorageFolder folder)
                await foreach (var subItem in GetAllFilesAsync(folder))
                    yield return subItem;
        }
    }
}