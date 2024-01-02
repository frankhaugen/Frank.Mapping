namespace Frank.Mapping;

/// <summary>
/// Represents a mapping interface to convert objects from type TFrom to type TTo.
/// </summary>
/// <typeparam name="TFrom">The type of object to be converted.</typeparam>
/// <typeparam name="TTo">The type of converted object.</typeparam>
public interface IMappingDefinition<TFrom, TTo>
{
    /// <summary>
    /// Maps an instance of type TFrom to an instance of type TTo.
    /// </summary>
    /// <typeparam name="TFrom">The source type to map from.</typeparam>
    /// <typeparam name="TTo">The target type to map to.</typeparam>
    /// <param name="from">The object of type TFrom to be mapped.</param>
    /// <returns>The mapped object of type TTo.</returns>
    TTo Map(TFrom from);
}