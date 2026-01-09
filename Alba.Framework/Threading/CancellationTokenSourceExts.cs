namespace Alba.Framework.Threading;

public static class CancellationTokenSourceExts
{
    extension(CancellationTokenSource? @this)
    {
        public void CancelAndDispose()
        {
            if (@this != null) {
                @this.Cancel();
                @this.Dispose();
            }
        }

        public async ValueTask CancelAndDisposeAsync()
        {
            if (@this != null) {
                await @this.CancelAsync();
                @this.Dispose();
            }
        }
    }
}