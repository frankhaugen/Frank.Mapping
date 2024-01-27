namespace Frank.Mapping;

internal class SimpleMapping<T1, T2>(Func<T1, T2> map) : IMappingDefinition<T1, T2>
    where T1 : class
    where T2 : class
{
    /// <inheritdoc />
    public T2 Map(T1 from) => map(from);
}