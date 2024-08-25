namespace Frank.Mapping;

/// <summary>
/// Represents a mapping interface to convert objects from type TSource to type TDestination.
/// </summary>
/// <typeparam name="TSource">The type of object to be converted.</typeparam>
/// <typeparam name="TDestination">The type of converted object.</typeparam>
public interface IMappingDefinition<TSource, TDestination>
{
    /// <summary>
    /// Maps an instance of type TSource to an instance of type TDestination.
    /// </summary>
    /// <typeparam name="TSource">The source type to map from.</typeparam>
    /// <typeparam name="TDestination">The target type to map to.</typeparam>
    /// <param name="source">The object of type TSource to be mapped.</param>
    /// <returns>The mapped object of type TDestination.</returns>
    TDestination Map(TSource source);
}