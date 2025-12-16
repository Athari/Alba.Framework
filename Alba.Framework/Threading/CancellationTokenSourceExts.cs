namespace Alba.Framework.Threading;

public static class CancellationTokenSourceExts
{
    extension(CancellationTokenSource? @this)
    {
        public async Task CancelAndDisposeAsync()
        {
            if (@this != null) {
                await @this.CancelAsync();
                @this.Dispose();
            }
        }
    }
}