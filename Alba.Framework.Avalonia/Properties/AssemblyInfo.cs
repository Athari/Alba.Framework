using System.Diagnostics.CodeAnalysis;
using Avalonia.Metadata;
using static IS;

[assembly: XmlnsPrefix(UrnGui, PrefixGui)]
[assembly: XmlnsDefinition(UrnGui, $"{NsGui}")]
[assembly: XmlnsDefinition(UrnGui, $"{NsGui}.Controls")]
[assembly: XmlnsDefinition(UrnGui, $"{NsGui}.Markup.Converters")]
[assembly: XmlnsDefinition(UrnGui, $"{NsGui}.Markup.MarkupExtensions")]

[SuppressMessage("ReSharper", "CheckNamespace")]
file interface IS
{
    const string UrnGui = "urn:alba:avalonia";
    const string NsGui = "Alba.Framework.Avalonia";
    const string PrefixGui = "aa";
}