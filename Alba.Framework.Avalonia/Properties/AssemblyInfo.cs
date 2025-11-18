using Avalonia.Metadata;
using static IS;

[assembly: XmlnsPrefix(UrnAlba, PrefixAlba)]
[assembly: XmlnsDefinition(UrnAlba, $"{NsAlba}")]
[assembly: XmlnsDefinition(UrnAlba, $"{NsAlba}.Controls")]
[assembly: XmlnsDefinition(UrnAlba, $"{NsAlba}.Markup.Converters")]
[assembly: XmlnsDefinition(UrnAlba, $"{NsAlba}.Markup.MarkupExtensions")]

[SuppressMessage("ReSharper", "CheckNamespace")]
file interface IS
{
    const string PrefixAlba = "aa";
    const string NsAlba = "Alba.Framework.Avalonia";
    const string UrnAlba = "urn:alba:avalonia";
}