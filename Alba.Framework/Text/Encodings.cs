using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Alba.Framework.Text;

[PublicAPI]
public static class Encodings
{
    [field: MaybeNull]
    public static UTF8Encoding Utf8 => field ??= new(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: false);
    [field: MaybeNull]
    public static UTF8Encoding Utf8Strict => field ??= new(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: true);
    [field: MaybeNull]
    public static UTF8Encoding Utf8Bom => field ??= new(encoderShouldEmitUTF8Identifier: true, throwOnInvalidBytes: false);
    [field: MaybeNull]
    public static UTF8Encoding Utf8BomStrict => field ??= new(encoderShouldEmitUTF8Identifier: true, throwOnInvalidBytes: true);
    [field: MaybeNull]
    public static UnicodeEncoding Utf16 => field ??= new(bigEndian: false, byteOrderMark: false, throwOnInvalidBytes: false);
    [field: MaybeNull]
    public static UnicodeEncoding Utf16Strict => field ??= new(bigEndian: false, byteOrderMark: false, throwOnInvalidBytes: true);
    [field: MaybeNull]
    public static UnicodeEncoding Utf16Bom => field ??= new(bigEndian: false, byteOrderMark: true, throwOnInvalidBytes: false);
    [field: MaybeNull]
    public static UnicodeEncoding Utf16BomStrict => field ??= new(bigEndian: false, byteOrderMark: true, throwOnInvalidBytes: true);
    [field: MaybeNull]
    public static UnicodeEncoding Utf16BigEndian => field ??= new(bigEndian: true, byteOrderMark: false, throwOnInvalidBytes: false);
    [field: MaybeNull]
    public static UnicodeEncoding Utf16BigEndianStrict => field ??= new(bigEndian: true, byteOrderMark: false, throwOnInvalidBytes: true);
    [field: MaybeNull]
    public static UnicodeEncoding Utf16BigEndianBom => field ??= new(bigEndian: true, byteOrderMark: true, throwOnInvalidBytes: false);
    [field: MaybeNull]
    public static UnicodeEncoding Utf16BigEndianBomStrict => field ??= new(bigEndian: true, byteOrderMark: true, throwOnInvalidBytes: true);
}