namespace Frank.Mapping;

public interface IMappingProvider
{
    /// <summary>
    /// Maps an object of type TFrom to an object of type TTo.
    /// </summary>
    /// <typeparam name="TFrom">The type of the object to map from.</typeparam>
    /// <typeparam name="TTo">The type of the object to map to.</typeparam>
    /// <param name="from">The object to map from.</param>
    /// <returns>The mapped object of type TTo.</returns>
    public TTo Map<TFrom, TTo>(TFrom from);

    /// <summary>
    /// Retrieves the mapping definition for the specified source and destination types.
    /// </summary>
    /// <typeparam name="TFrom">The source type.</typeparam>
    /// <typeparam name="TTo">The destination type.</typeparam>
    /// <returns>The mapping definition for the specified types.</returns>
    public IMappingDefinition<TFrom, TTo> GetMappingDefinition<TFrom, TTo>();
}