using Avalonia.Platform.Storage;

namespace Alba.Framework.Avalonia;

public static class StorageExts
{
    public static string GetExtension(this IStorageItem @this) =>
        Path.GetExtension(@this.Name);

    public static string GetNameWithoutExtension(this IStorageItem @this) =>
        Path.GetFileNameWithoutExtension(@this.Name);

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