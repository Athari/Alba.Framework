using System.Reflection;

namespace Alba.Framework.Reflection;

public static class AssemblyNameExts
{
    private static readonly byte[] MicrosoftPublicKeyToken = BitConverter.GetBytes(0x_354E_36AD_5638_BF31);

    extension(AssemblyName @this)
    {
        public AssemblyName WithPublicKeyToken(byte[] token)
        {
            @this.SetPublicKeyToken(token);
            return @this;
        }

        public AssemblyName WithMicrosoftPublicKeyToken() =>
            @this.WithPublicKeyToken(MicrosoftPublicKeyToken);

        public Uri ToPackUri(string resourcePath) =>
            new($"/{@this};component{resourcePath}", UriKind.Relative);
    }
}