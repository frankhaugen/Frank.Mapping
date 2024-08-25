using System.Text.Json;
using Frank.Mapping.Documents.Models.Enums;

namespace Frank.Mapping.Documents.Helpers;

public static class JsonPathHelper
{
    public static IEnumerable<string> GetPaths(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value, nameof(value));

        var documentVariant = DocumentVariantHelper.GetDocumentVariant(value);

        if (documentVariant != DocumentVariant.Json)
            throw new ArgumentException("Value is not a JSON document.");

        var json = JsonDocument.Parse(value);
        return ExtractAllJsonPaths(json);
    }

    private static IEnumerable<string> ExtractAllJsonPaths(JsonDocument json)
    {
        var paths = new List<string>();
        ExtractAllJsonPaths(json.RootElement, string.Empty, paths);
        return paths;
    }

    private static void ExtractAllJsonPaths(JsonElement jsonRootElement, string empty, List<string> paths)
    {
        // Extract all JSON paths from the JSON document. JSON paths are used to map JSON properties to C# properties. The syntax is similar to XPath for XML, example: $.Name or $.Address.City, or $.[0].Name or $.[0].Address.City, or $.Address[0].City.
        // The method is recursive and will traverse all JSON elements in the JSON document.
        // The method is called by the GetPaths method in the JsonPathHelper class.
        // The method is called by the ExtractAllJsonPaths method in the JsonPathHelper class.
        
        if (jsonRootElement.ValueKind == JsonValueKind.Object)
        {
            foreach (var property in jsonRootElement.EnumerateObject())
            {
                var path = $"{empty}.{property.Name}";
                paths.Add($"${path}");
                ExtractAllJsonPaths(property.Value, path, paths);
            }
        }
        else if (jsonRootElement.ValueKind == JsonValueKind.Array)
        {
            for (var i = 0; i < jsonRootElement.GetArrayLength(); i++)
            {
                var path = "{empty}[{i}]";
                paths.Add($"${path}");
                ExtractAllJsonPaths(jsonRootElement[i], path, paths);
            }
        }
    }
}