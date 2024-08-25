namespace Frank.Mapping;

/// <summary>
/// Represents an asynchronous mapping between two types. You can use this interface to map from one type to another asynchronously, for example if you need to make a web request to get the data to map to.
/// </summary>
/// <typeparam name="TSource">The source type to map from.</typeparam>
/// <typeparam name="TDestination">The destination type to map to.</typeparam>
public interface IAsyncMappingDefinition<TSource, TDestination>
{
    /// <summary>
    /// Maps an object of type TSource to an object of type TDestination asynchronously.
    /// </summary>
    /// <typeparam name="TSource">The type of the object to be mapped from.</typeparam>
    /// <typeparam name="TDestination">The type of the object to be mapped to.</typeparam>
    /// <param name="source">The object to be mapped.</param>
    /// <returns>A task representing the asynchronous operation, which will eventually contain the mapped object of type TDestination.</returns>
    Task<TDestination> MapAsync(TSource source);
}