using System.Diagnostics;
using System.Text.Json;
using System.Xml;
using Json.More;
using Json.Path;

namespace Frank.Mapping.Documents;

[DebuggerDisplay("{DocumentVariant} - {Path} - {ValueType}")]
public class ValuePath
{
    public DocumentVariant DocumentVariant { get; }
    public string Path { get; }
    public Type ValueType { get; }

    public ValuePath(DocumentVariant documentVariant, string path, Type valueType)
    {
        DocumentVariant = documentVariant;
        Path = path ?? throw new ArgumentNullException(nameof(path));
        ValueType = valueType ?? throw new ArgumentNullException(nameof(valueType));
        
        if (DocumentVariant == DocumentVariant.Json && JsonPath.TryParse(Path, out _) == false)
            throw new ArgumentException($"Invalid path: {Path}", nameof(path));

        if (DocumentVariant == DocumentVariant.Xml)
        {
            try
            {
                new XmlDocument().SelectSingleNode(Path);
            }
            catch (Exception e)
            {
                throw new ArgumentException($"Invalid path: {Path}", nameof(path), e);
            }
        }
    }
    
    public object? GetValue(string document)
    {
        return DocumentVariant switch
        {
            DocumentVariant.Json => GetValueFromJson(document),
            DocumentVariant.Xml => GetValueFromXml(document),
            _ => throw new ArgumentOutOfRangeException(nameof(DocumentVariant), DocumentVariant, null)
        };
    }

    /// <inheritdoc />
    public override string ToString() => $"{DocumentVariant} - {Path} - {ValueType}";

    private object? GetValueFromXml(string document)
    {
        var xmlDocument = new XmlDocument();
        xmlDocument.LoadXml(document);
        var xmlNode = xmlDocument.SelectSingleNode(Path);
        
        if (xmlNode == null)
            return default;
        
        return Convert.ChangeType(xmlNode.InnerText, ValueType);
    }

    private object? GetValueFromJson(string document)
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
        var node = match.Value;
        if (node == null)
            return default;
        
        if (node.GetValueKind() == JsonValueKind.Null)
            return default;
        
        try
        {
            var value = node.AsValue();
            return node.GetValueKind() switch
            {
                JsonValueKind.String => value.GetString(),
                JsonValueKind.Number => value.GetNumber(),
                JsonValueKind.True => true,
                JsonValueKind.False => false,
                JsonValueKind.Null => null,
                JsonValueKind.Undefined or JsonValueKind.Object or JsonValueKind.Array => throw new InvalidOperationException($"Unsupported value kind: {node.GetValueKind()}"),
                _ => throw new InvalidOperationException($"Unsupported value kind: {node.GetValueKind()}")
            };
        }
        catch (Exception e)
        {
            return default;
        }
    }
}