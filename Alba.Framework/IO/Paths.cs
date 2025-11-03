using System.Reflection;
using System.Text.RegularExpressions;
using Alba.Framework.Collections;
using Alba.Framework.Text;

namespace Alba.Framework.IO;

[PublicAPI]
public static partial class Paths
{
    private const string InvalidCharString = "�";
    private const RegexOptions SimpleReOpts = RegexOptions.Compiled | RegexOptions.CultureInvariant;
    private static char[]? AllInvalidPathChars;

    [GeneratedRegex($"[{InvalidCharString}+]", SimpleReOpts)]
    private static partial Regex ReInvalidChars { get; }

    [field: MaybeNull]
    private static Regex ReInvalidPathChar => field ??= new("[" + Regex.Escape(new(GetInvalidPathChars())) + "]", SimpleReOpts);

    [field: MaybeNull]
    public static string ExecutableFilePath => field ??= Assembly.GetExecutingAssembly().Location;

    [field: MaybeNull]
    public static string ExecutableFileDir => field ??= Path.GetDirectoryName(ExecutableFilePath) ?? ".";

    [Pure]
    public static string ChangeDir(string path,
        Func<string[], IEnumerable<string?>> dirParts,
        Func<string, string>? safe = null,
        PathConcat concat = PathConcat.Join)
    {
        Func<IEnumerable<string?>, string> callCombine = concat switch {
            PathConcat.Combine => Combine,
            #pragma warning disable CS0618 // Type or member is obsolete
            PathConcat.CombineSafe => CombineSafe,
            #pragma warning restore CS0618
            PathConcat.Join => Join,
            _ => throw EnumException.Create(nameof(concat), concat),
        };
        var outParts = dirParts(Split(path));
        if (safe != null)
            outParts = outParts.Select(p => p != null ? safe(p) : p);
        return callCombine(outParts);
    }

    [Pure]
    public static string ChangeExt(string path, string? ext) => Path.ChangeExtension(path, ext);

    [Pure]
    public static string ChangePath(string path,
        Func<string[], IEnumerable<string?>>? dirParts = null,
        Func<string, string?>? dir = null,
        Func<string, string?>? name = null,
        Func<string, string?>? ext = null,
        Func<string, string>? safe = null,
        PathConcat concat = PathConcat.Join)
    {
        Func<string?, string?, string> callCombine = concat switch {
            PathConcat.Combine => Combine,
            #pragma warning disable CS0618 // Type or member is obsolete
            PathConcat.CombineSafe => CombineSafe,
            #pragma warning restore CS0618
            PathConcat.Join => Path.Join,
            _ => throw EnumException.Create(nameof(concat), concat),
        };

        var inDir = Path.GetDirectoryName(path);
        var inName = GetNameWithoutExt(path);
        var inExt = GetExt(path);

        string? outDir;
        if (inDir == null)
            outDir = null;
        else if (dirParts != null)
            outDir = ChangeDir(inDir, dirParts, safe, concat);
        else
            outDir = inDir;
        if (outDir != null)
            outDir = dir?.Invoke(outDir) ?? outDir;

        var outName = name?.Invoke(inName) ?? inName;
        var outExt = ext?.Invoke(inExt) ?? inExt;
        var outNameExt = $"{outName}{outExt}";

        return callCombine(outDir, safe != null ? safe(outNameExt) : outNameExt);
    }

    [Pure, Obsolete("Use Path.Join")]
    public static string CombineSafe(string? path1, string? path2) =>
        (path1, path2) switch {
            ({ } p1, { } p2) => Path.Combine(p1, p2),
            ({ } p1, null) => p1,
            (null, { } p2) => p2,
            (null, null) => "",
        };

    [Pure, Obsolete("Use Path.Join")]
    public static string CombineSafe(string? path1, string? path2, string? path3) =>
        (path1, path2, path3) switch {
            ({ } p1, { } p2, { } p3) => Path.Combine(p1, p2, p3),
            ({ } p1, { } p2, null) => Path.Combine(p1, p2),
            ({ } p1, null, { } p3) => Path.Combine(p1, p3),
            ({ } p1, null, null) => p1,
            (null, { } p2, { } p3) => Path.Combine(p2, p3),
            (null, { } p2, null) => p2,
            (null, null, { } p3) => p3,
            (null, null, null) => "",
        };

    [Pure, Obsolete("Use Path.Join")]
    public static string CombineSafe(string? path1, string? path2, string? path3, string? path4) =>
        (path1, path2, path3, path4) switch {
            ({ } p1, { } p2, { } p3, { } p4) => Path.Combine(p1, p2, p3, p4),
            ({ } p1, { } p2, { } p3, null) => Path.Combine(p1, p2, p3),
            ({ } p1, { } p2, null, { } p4) => Path.Combine(p1, p2, p4),
            ({ } p1, { } p2, null, null) => Path.Combine(p1, p2),
            ({ } p1, null, { } p3, { } p4) => Path.Combine(p1, p3, p4),
            ({ } p1, null, { } p3, null) => Path.Combine(p1, p3),
            ({ } p1, null, null, { } p4) => Path.Combine(p1, p4),
            ({ } p1, null, null, null) => p1,
            (null, { } p2, { } p3, { } p4) => Path.Combine(p2, p3, p4),
            (null, { } p2, { } p3, null) => Path.Combine(p2, p3),
            (null, { } p2, null, { } p4) => Path.Combine(p2, p4),
            (null, { } p2, null, null) => p2,
            (null, null, { } p3, { } p4) => Path.Combine(p3, p4),
            (null, null, { } p3, null) => p3,
            (null, null, null, { } p4) => p4,
            (null, null, null, null) => "",
        };

    [Pure, Obsolete("Use Path.Join")]
    public static string CombineSafe(params IEnumerable<string?> paths)
    {
        var actualPaths = paths.WhereNotNull().ToArray();
        return actualPaths.Length > 0 ? Path.Combine(actualPaths) : "";
    }

    [SuppressMessage("ReSharper", "RedundantRangeBound", Justification = "Consistency")]
    [SuppressMessage("Style", "IDE0305:Simplify collection initialization", Justification = "LINQ")]
    public static char[] GetInvalidPathChars(bool includeControl = true)
    {
        // See Rune.IsControl for explanation
        return !includeControl
            ? Path.GetInvalidPathChars()
            : AllInvalidPathChars ??= Path.GetInvalidPathChars()
                .Concat(CharRange(0x00..0x1F)).Concat(CharRange(0x7F..0x9F))
                .ToArray();

        static IEnumerable<char> CharRange(Range range) =>
            range.ToRange(endInclusive: true).Select(Convert.ToChar);
    }

    [Pure]
    public static string GetExt(string path) =>
        Path.GetExtension(path);

    [Pure]
    public static string GetName(string path) =>
        Path.GetFileName(path);

    [Pure]
    public static string GetNameWithoutExt(string path) =>
        Path.GetFileNameWithoutExtension(path);

    [Pure]
    public static string GetDirName(string path) =>
        Path.GetDirectoryName(path) ?? throw new ArgumentOutOfRangeException(nameof(path), path, "Path is a root directory");

    [Pure]
    public static string GetTempFileName(string ext) =>
        Path.Join(Path.GetTempPath(), $"{Guid.NewGuid():N}.{ext}");

    public static string GetRoamingAppDir(string company, string product) =>
        GetAppDir(Environment.SpecialFolder.ApplicationData.GetPath(), company, product);

    public static string GetLocalAppDir(string company, string product) =>
        GetAppDir(Environment.SpecialFolder.LocalApplicationData.GetPath(), company, product);

    public static string GetCommonAppDir(string company, string product) =>
        GetAppDir(Environment.SpecialFolder.CommonApplicationData.GetPath(), company, product);

    public static string GetPortableAppDir(params IEnumerable<string?> parts) =>
        GetAppDir(ExecutableFileDir, parts);

    private static string GetAppDir(string baseDir, params IEnumerable<string?> parts)
    {
        string dir = Path.Join([ baseDir, ..parts ]);
        Directory.CreateDirectory(dir);
        return dir;
    }

    public static string GetPath(this Environment.SpecialFolder @this,
        Environment.SpecialFolderOption option = Environment.SpecialFolderOption.Create) =>
        Environment.GetFolderPath(@this, option);

    [Pure]
    public static string GetSafeName(string fileName, MatchEvaluator? replace = null, MatchEvaluator? replaceInvalid = null) =>
        fileName
            .ReReplace(ReInvalidPathChar, replace ?? (_ => InvalidCharString))
            .ReReplace(ReInvalidChars, replaceInvalid ?? (_ => "_"));

    [Pure]
    public static string[] Split(string path) =>
        path.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);

    [Pure]
    private static string Combine(IEnumerable<string?> parts) =>
        Path.Combine([ ..parts.Cast<string>() ]); // let it throw

    [Pure]
    private static string Combine(string? path1, string? path2) =>
        Path.Combine(path1!, path2!); // let it throw

    [Pure]
    private static string Join(IEnumerable<string?> parts) =>
        Path.Join([ ..parts ]);
}