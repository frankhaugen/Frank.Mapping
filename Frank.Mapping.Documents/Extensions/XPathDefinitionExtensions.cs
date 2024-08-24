using System.Xml;
using System.Xml.Serialization;
using Frank.Mapping.Documents.Path;

namespace Frank.Mapping.Documents.Extensions;

public static class XPathDefinitionExtensions
{
    public static T? GetValue<T>(this XPathDefinition xPathDefinition, object? source)
    {
        ArgumentNullException.ThrowIfNull(xPathDefinition, nameof(xPathDefinition));
        
        if (source is string sourceString)
            return xPathDefinition.GetValue<T>(sourceString);
        
        if (source is XmlDocument sourceXmlDocument)
            return xPathDefinition.GetValue<T>(sourceXmlDocument);
        
        var xmlDocument = new XmlDocument();

        XmlSerializer xmlSerializer = new XmlSerializer(source!.GetType());
        using var writer = new StringWriter();
        xmlSerializer.Serialize(writer, source);
        xmlDocument.LoadXml(writer.ToString());
        
        var xmlNode = xmlDocument.SelectSingleNode(xPathDefinition.Path);
        
        if (xmlNode == null)
            return default;
        
        return (T)Convert.ChangeType(xmlNode.InnerText, typeof(T));
    }
}