using System.Reflection;

namespace Alba.Framework.Reflection;

public static class AssemblyNameExts
{
    private static readonly byte[] MicrosoftPublicKeyToken = BitConverter.GetBytes(0x_354E_36AD_5638_BF31);

    public static AssemblyName WithPublicKeyToken(this AssemblyName @this, byte[] token)
    {
        @this.SetPublicKeyToken(token);
        return @this;
    }

    public static AssemblyName WithMicrosoftPublicKeyToken(this AssemblyName @this) =>
        @this.WithPublicKeyToken(MicrosoftPublicKeyToken);

    public static Uri ToPackUri(this AssemblyName @this, string resourcePath) =>
        new($"/{@this};component{resourcePath}", UriKind.Relative);
}