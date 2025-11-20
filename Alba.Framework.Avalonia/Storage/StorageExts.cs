using Alba.Framework.IO;
using Avalonia.Platform.Storage;

namespace Alba.Framework.Avalonia.Storage;

public static class StorageExts
{
    extension(IStorageItem @this)
    {
        public string LocalPath =>
            OperatingSystem.IsAndroid() ? @this.Path.AbsoluteUri : @this.Path.LocalPath;

        public string Ext =>
            Paths.GetExt(@this.Name);

        public string NameWithoutExt =>
            Paths.GetNameWithoutExt(@this.Name);

        public string ChangeName(
            Func<string, string?>? name = null,
            Func<string, string?>? ext = null)
        {
            var itemName = @this.NameWithoutExt;
            var itemExt = @this.Ext;
            return $"{name?.Invoke(itemName) ?? itemName}{ext?.Invoke(itemExt) ?? itemExt}";
        }
    }

    extension(IStorageFolder @this)
    {
        public async IAsyncEnumerable<IStorageFile> GetAllFilesAsync()
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
}