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
        switch (jsonRootElement.ValueKind)
        {
            case JsonValueKind.Object:
                foreach (var property in jsonRootElement.EnumerateObject())
                    ExtractAllJsonPaths(property.Value, $"{empty}/{property.Name}", paths);
                break;
            case JsonValueKind.Array:
                for (var i = 0; i < jsonRootElement.GetArrayLength(); i++)
                    ExtractAllJsonPaths(jsonRootElement[i], $"{empty}/{i}", paths);
                break;
            default:
                paths.Add(empty);
                break;
        }
    }
}