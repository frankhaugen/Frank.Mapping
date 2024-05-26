using System.Diagnostics;
using System.Text.Json;
using System.Xml;
using Frank.Mapping.Documents.Models.Enums;
using Json.More;
using Json.Path;

namespace Frank.Mapping.Documents.Models;

[DebuggerDisplay("{DocumentVariant} - {Path} - {ValueType}")]
public class ValuePath<T>(DocumentVariant documentVariant, string path) : ValuePath(documentVariant, path, typeof(T))
{
    public new T? GetValue(string document)
    {
        return DocumentVariant switch
        {
            DocumentVariant.Json => GetValueFromJson(document),
            DocumentVariant.Xml => GetValueFromXml(document),
            _ => throw new ArgumentOutOfRangeException(nameof(DocumentVariant), DocumentVariant, null)
        };
    }

    private T? GetValueFromXml(string document)
    {
        var xmlDocument = new XmlDocument();
        xmlDocument.LoadXml(document);
        var xmlNode = xmlDocument.SelectSingleNode(Path);
        
        if (xmlNode == null)
            return default;
        
        return (T)Convert.ChangeType(xmlNode.InnerText, typeof(T));
    }

    private T? GetValueFromJson(string document)
    {
        var jsonPath = JsonPath.Parse(Path);
        var jsonDocument = JsonDocument.Parse(document);
        var result = jsonPath.Evaluate(jsonDocument.RootElement.AsNode());
        
        if (result.Error != null)
            throw new InvalidOperationException($"Failed to evaluate path: {Path} with errors: {result.Error}");
        
        var matches = result.Matches;
        
        if (matches == null)
            return default;
        if (matches.Count == 0)
            return default;
        if (matches.Count > 1)
            throw new InvalidOperationException($"Multiple matches found for path: {Path}, matches: {matches}");
        
        var match = matches[0];
        var value = match.Value;
        if (value == null)
            return default;
        
        if (value.GetValueKind() == JsonValueKind.Null)
            return default;

        return value.GetValue<T>();
    }
}