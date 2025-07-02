using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using Windows.Win32;

namespace Alba.Framework.Win32;

[Guid(GuidString)]
[SuppressMessage("ReSharper", "SuspiciousTypeConversion.Global", Justification = "COM interfaces")]
internal class PersistentZoneIdentifier : ComWrapper<IZoneIdentifier>, ITypeHasGuid
{
    private const string GuidString = "0968E258-16C7-4DBA-AA86-462DD61E31A3";
    public static readonly Guid Guid = new(GuidString);
    static Guid ITypeHasGuid.Guid => Guid;

    public IZoneIdentifier Zone => Com;

    [field: MaybeNull]
    public IPersistFile File => field ??= (IPersistFile)Com;

    public static PersistentZoneIdentifier Create() => Native.CoCreateInstance<PersistentZoneIdentifier, IZoneIdentifier>();
}