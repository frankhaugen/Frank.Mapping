namespace Frank.Mapping.Documents.Path;

public record ConfigPathDefinition : PathDefinition
{
    public ConfigPathDefinition(string path) : base(path)
    {
        EnsureValidConfigPath(path);
    }

    private static void EnsureValidConfigPath(string path)
    {
        if (path.Contains('.'))
            throw new ArgumentException($"The provided config path '{path}' is invalid. Config paths should not contain dots.", nameof(path));
    }
}