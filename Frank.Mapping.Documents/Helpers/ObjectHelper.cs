using Microsoft.Extensions.Configuration;

namespace Frank.Mapping.Documents.Helpers;

public static class ObjectHelper
{
    public static T CreateInstance<T>() => Activator.CreateInstance<T>();

    public static T CreateInstanceFromValues<T>(IDictionary<string, string?> data)
    {
        ArgumentNullException.ThrowIfNull(data);

        var instance = CreateInstance<T>();
        AssignValues(instance, data);
        return instance;
    }
    
    public static void AssignValues<T>(T instance, IDictionary<string, string?> data)
    {
        ArgumentNullException.ThrowIfNull(instance);
        ArgumentNullException.ThrowIfNull(data);

        var configurationBuilder = new ConfigurationBuilder().AddInMemoryCollection(data);
        var configuration = configurationBuilder.Build();
        configuration.Bind(instance);
    }
}