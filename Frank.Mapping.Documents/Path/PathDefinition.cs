namespace Frank.Mapping.Documents.Path;

public abstract record PathDefinition : IPathDefinition
{
    public string Path { get; }

    protected PathDefinition(string path)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(path, nameof(path));
        Path = path;
    }
    
    public override string ToString() => Path;
    
    public static implicit operator string(PathDefinition pathDefinition) => pathDefinition.Path;
    
    public static T? Create<T>(string path) where T : PathDefinition => Activator.CreateInstance(typeof(T), path) as T;
}