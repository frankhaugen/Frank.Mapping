using System.Diagnostics;

namespace Frank.Mapping.Documents.Models;

[DebuggerDisplay("{GetPath} => {Value}")]
public class ValuePathResult(ValuePath valuePath, object? value)
{
    public ValuePath ValuePath { get; } = valuePath;
    public object? Value { get; } = value;

    /// <inheritdoc />
    public override string ToString() => $"'{ValuePath.Path}' => '{Value}'";

    public T? GetValue<T>() => (T?)Value;
    
    public string GetPath() => ValuePath.Path;
}