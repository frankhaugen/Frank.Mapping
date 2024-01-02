using Microsoft.Extensions.DependencyInjection;

namespace Frank.Mapping;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds a mapping between types <typeparamref name="TFrom"/> and <typeparamref name="TTo"/>
    /// using the provided implementation of <typeparamref name="TMapping"/>.
    /// </summary>
    /// <param name="services">The service collection to add the mapping to.</param>
    /// <typeparam name="TFrom">The source type of the mapping.</typeparam>
    /// <typeparam name="TTo">The destination type of the mapping.</typeparam>
    /// <typeparam name="TMapping">The implementation of the mapping.</typeparam>
    /// <returns>The modified service collection with the mapping added.</returns>
    public static IServiceCollection AddMappingDefinition<TFrom, TTo, TMapping>(this IServiceCollection services) where TMapping : class, IMappingDefinition<TFrom, TTo>
    {
        services.AddSingleton<IMappingDefinition<TFrom, TTo>, TMapping>();
        services.AddSingleton<IMappingProvider, MappingProvider>();
        return services;
    }

    /// <summary>
    /// Adds a singleton instance of a class implementing the <see cref="IAsyncMappingDefinition{TFrom,TTo}"/> interface to the <see cref="IServiceCollection"/>.
    /// </summary>
    /// <typeparam name="TFrom">The source type.</typeparam>
    /// <typeparam name="TTo">The destination type.</typeparam>
    /// <typeparam name="TMapping">The type implementing the <see cref="IAsyncMappingDefinition{TFrom,TTo}"/> interface.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the mapping to.</param>
    /// <returns>The modified <see cref="IServiceCollection"/> instance.</returns>
    public static IServiceCollection AddAsyncMappingDefinition<TFrom, TTo, TMapping>(this IServiceCollection services) where TMapping : class, IAsyncMappingDefinition<TFrom, TTo>
    {
        services.AddSingleton<IAsyncMappingDefinition<TFrom, TTo>, TMapping>();
        services.AddSingleton<IMappingProvider, MappingProvider>();
        return services;
    }
}