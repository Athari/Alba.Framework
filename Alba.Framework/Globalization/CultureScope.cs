using System.Globalization;

namespace Alba.Framework.Globalization;

public sealed class CultureScope : IDisposable
{
    private readonly CultureInfo _oldCulture;

    public CultureScope(CultureInfo? culture = null)
    {
        _oldCulture = CultureInfo.CurrentCulture;
        CultureInfo.CurrentCulture = culture ?? CultureInfo.InvariantCulture;
    }

    public void Dispose()
    {
        CultureInfo.CurrentCulture = _oldCulture;
    }
}