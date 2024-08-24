using System.ComponentModel;

namespace Frank.Mapping.Documents.Path;

public interface IPathDefinition : IEquatable<PathDefinition?>
{
    /// <summary>
    /// The path.
    /// </summary>
    string Path { get; }
    
    /// <summary>
    /// Determines whether the specified object is equal to the current object.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    [EditorBrowsable(EditorBrowsableState.Never)]
    bool Equals(PathDefinition? other);
    
    /// <summary>
    /// Determines whether the specified object is equal to the current object.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    [EditorBrowsable(EditorBrowsableState.Never)]
    bool Equals(object? other);
    
    /// <summary>
    /// Returns the hash code of the path.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    int GetHashCode();
    
    /// <summary>
    /// Returns the path as a string.
    /// </summary>
    string ToString();
}