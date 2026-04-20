using Microsoft.Extensions.DependencyInjection;

namespace Frank.Mapping.Tests;

public class ServiceCollectionExtensionsTests
{
    [Test]
    public async Task AddSimpleMapping_ShouldAddMappingDefinitionToServices()
    {
        // Arrange
        var services = new ServiceCollection();
        Func<Version, string> map = version => version.ToString(2);

        // Act
        services.AddSimpleMapping<Version, string>(map);

        // Assert
        var serviceProvider = services.BuildServiceProvider();
        var mappingDefinition = serviceProvider.GetService<IMappingDefinition<Version, string>>();
        await Assert.That(mappingDefinition).IsNotNull();
        
        var result = mappingDefinition!.Map(new Version(1, 2, 3, 4));
        Console.WriteLine(result);
        await Assert.That(result).IsEqualTo("1.2");
    }

    [Test]
    public async Task AddSimpleMapping_ShouldAddMappingProviderToServices()
    {
        // Arrange
        var services = new ServiceCollection();
        Func<Version, string> map = version => version.ToString(2);

        // Act
        services.AddSimpleMapping<Version, string>(map);

        // Assert
        var serviceProvider = services.BuildServiceProvider();
        var mappingProvider = serviceProvider.GetService<IMappingProvider>();
        await Assert.That(mappingProvider).IsNotNull();
        
        var result = mappingProvider!.Map<Version, string>(new Version(1, 2, 3, 4));
        Console.WriteLine(result);
        await Assert.That(result).IsEqualTo("1.2");
    }

    [Test]
    public async Task AddSimpleMapping_ShouldThrowException_WhenMapIsNull()
    {
        // Arrange
        var services = new ServiceCollection();
        Func<Version, string> map = null!;

        // Act & Assert
        await Assert.That(() => services.AddSimpleMapping<Version, string>(map)).ThrowsExactly<ArgumentNullException>();
    }
    
    [Test]
    public async Task AddAsyncMappingDefinition_ShouldAddMappingDefinitionToServices()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddAsyncMappingDefinition<Version, string, AsyncMapping>();

        // Assert
        var serviceProvider = services.BuildServiceProvider(new ServiceProviderOptions() { ValidateOnBuild = true, ValidateScopes = true });
        var mappingDefinition = serviceProvider.GetService<IAsyncMappingDefinition<Version, string>>();
        await Assert.That(mappingDefinition).IsNotNull();
        
        var result = await mappingDefinition!.MapAsync(new Version(1, 2, 3, 4));
        Console.WriteLine(result);
        await Assert.That(result).IsEqualTo("1.2");
    }

    public class AsyncMapping : IAsyncMappingDefinition<Version, string>
    {
        public Task<string> MapAsync(Version source)
        {
            return Task.FromResult(source.ToString(2));
        }
    }
}