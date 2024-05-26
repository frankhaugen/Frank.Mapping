namespace Frank.Mapping.Documents.Models;

public class ValueAction(ValuePath valuePath, Action<object?> action)
{
    public ValuePath ValuePath { get; } = valuePath;

    public Action<object?> Action { get; } = action;
}

public class ValueAction<T>(ValuePath valuePath, Action<T> action) : ValueAction(valuePath, o => action((T)o));