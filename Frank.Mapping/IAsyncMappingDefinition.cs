namespace Frank.Mapping;

/// <summary>
/// Represents an asynchronous mapping between two types. You can use this interface to map from one type to another asynchronously, for example if you need to make a web request to get the data to map to.
/// </summary>
/// <typeparam name="TFrom">The source type to map from.</typeparam>
/// <typeparam name="TTo">The destination type to map to.</typeparam>
public interface IAsyncMappingDefinition<TFrom, TTo>
{
    /// <summary>
    /// Maps an object of type TFrom to an object of type TTo asynchronously.
    /// </summary>
    /// <typeparam name="TFrom">The type of the object to be mapped from.</typeparam>
    /// <typeparam name="TTo">The type of the object to be mapped to.</typeparam>
    /// <param name="from">The object to be mapped.</param>
    /// <returns>A task representing the asynchronous operation, which will eventually contain the mapped object of type TTo.</returns>
    Task<TTo> MapAsync(TFrom from);
}