using Frank.Mapping.Documents.Models.Enums;
using Microsoft.Extensions.Configuration;

namespace Frank.Mapping.Documents.Helpers;

public static class ConfigPathHelper
{
    public static IEnumerable<string> GetPaths(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value, nameof(value));

        var documentVariant = DocumentVariantHelper.GetDocumentVariant(value);

        if (documentVariant != DocumentVariant.Json)
            throw new ArgumentException("Value is not a JSON document.");

        var configuration = BuildConfiguration(value);
        return ExtractAllConfigPaths(configuration);
    }

    private static IConfigurationRoot BuildConfiguration(string jsonContent)
    {
        var builder = new ConfigurationBuilder();
        builder.AddJsonStream(new MemoryStream(System.Text.Encoding.UTF8.GetBytes(jsonContent)));
        return builder.Build();
    }

    private static IEnumerable<string> ExtractAllConfigPaths(IConfiguration configuration)
    {
        var paths = new List<string>();
        ExtractAllConfigPaths(configuration, string.Empty, paths);
        return paths;
    }

    private static void ExtractAllConfigPaths(IConfiguration configuration, string parentPath, List<string> paths)
    {
        foreach (var child in configuration.GetChildren())
        {
            var currentPath = string.IsNullOrEmpty(parentPath) ? child.Key : $"{parentPath}:{child.Key}";
            paths.Add(currentPath);
            ExtractAllConfigPaths(child, currentPath, paths);
        }
    }
}