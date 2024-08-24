using System.Text.Json;
using Frank.Mapping.Documents.Path;
using Json.More;

namespace Frank.Mapping.Documents.Extensions;

public static class JsonPathDefinitionExtensions
{
    public static T? GetValue<T>(this JsonPathDefinition jsonPathDefinition, object instance)
    {
        var value = GetValue(jsonPathDefinition, instance);
        return value == null ? default : (T)value;
    }
    
    public static object? GetValue(this JsonPathDefinition jsonPathDefinition, object instance)
    {
        var jsonPath = jsonPathDefinition.JsonPath;
        var jsonDocument = JsonSerializer.SerializeToDocument(instance);
        var result = jsonPath.Evaluate(jsonDocument.RootElement.AsNode());
        
        if (result.Error != null)
            throw new InvalidOperationException($"Failed to evaluate path: {jsonPathDefinition} with errors: {result.Error}");
        
        var matches = result.Matches;
        
        if (matches == null)
            return default;
        if (matches.Count == 0)
            return default;
        if (matches.Count > 1)
            throw new InvalidOperationException($"Multiple matches found for path: {jsonPathDefinition}, matches: {matches}");
        
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