using System.IO;

namespace Alba.Framework.IO
{
    public static class StreamExts
    {
        public static Stream ToUndisposable (this Stream @this)
        {
            return new UndisposableStream(@this);
        }
    }
}