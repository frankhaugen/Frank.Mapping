using Json.Path;

namespace Frank.Mapping.Documents.Path;

public record JsonPathDefinition : PathDefinition
{
    public JsonPath JsonPath => JsonPath.Parse(Path);
    
    public JsonPathDefinition(string path) : base(path)
    {
        EnsureValidJsonPath(path);
    }

    private static void EnsureValidJsonPath(string path)
    {
        try
        {
            _ = JsonPath.Parse(path);
        }
        catch (PathParseException ex)
        {
            throw new ArgumentException($"The provided JSON path '{path}' is invalid.", nameof(path), ex);
        }
    }
}