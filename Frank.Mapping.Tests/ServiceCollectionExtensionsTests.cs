using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Frank.Mapping.Tests;

public class ServiceCollectionExtensionsTests(ITestOutputHelper outputHelper)
{
    private readonly ITestOutputHelper _outputHelper = outputHelper;

    [Fact]
    public void AddSimpleMapping_ShouldAddMappingDefinitionToServices()
    {
        // Arrange
        var services = new ServiceCollection();
        Func<Version, string> map = version => version.ToString(2);

        // Act
        services.AddSimpleMapping<Version, string>(map);

        // Assert
        var serviceProvider = services.BuildServiceProvider();
        var mappingDefinition = serviceProvider.GetService<IMappingDefinition<Version, string>>();
        Assert.NotNull(mappingDefinition);
        
        var result = mappingDefinition.Map(new Version(1, 2, 3, 4));
        _outputHelper.WriteLine(result);
        Assert.Equal("1.2", result);
    }

    [Fact]
    public void AddSimpleMapping_ShouldAddMappingProviderToServices()
    {
        // Arrange
        var services = new ServiceCollection();
        Func<Version, string> map = version => version.ToString(2);

        // Act
        services.AddSimpleMapping<Version, string>(map);

        // Assert
        var serviceProvider = services.BuildServiceProvider();
        var mappingProvider = serviceProvider.GetService<IMappingProvider>();
        mappingProvider.Should().NotBeNull();
        
        var result = mappingProvider.Map<Version, string>(new Version(1, 2, 3, 4));
        _outputHelper.WriteLine(result);
        result.Should().Be("1.2");
    }

    [Fact]
    public void AddSimpleMapping_ShouldThrowException_WhenMapIsNull()
    {
        // Arrange
        var services = new ServiceCollection();
        Func<Version, string> map = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => services.AddSimpleMapping<Version, string>(map));
    }
    
    [Fact]
    public async Task AddAsyncMappingDefinition_ShouldAddMappingDefinitionToServices()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddAsyncMappingDefinition<Version, string, AsyncMapping>();

        // Assert
        var serviceProvider = services.BuildServiceProvider(new ServiceProviderOptions() { ValidateOnBuild = true, ValidateScopes = true });
        var mappingDefinition = serviceProvider.GetService<IAsyncMappingDefinition<Version, string>>();
        Assert.NotNull(mappingDefinition);
        
        var result = await mappingDefinition.MapAsync(new Version(1, 2, 3, 4));
        _outputHelper.WriteLine(result);
        Assert.Equal("1.2", result);
    }

    public class AsyncMapping : IAsyncMappingDefinition<Version, string>
    {
        public Task<string> MapAsync(Version source)
        {
            return Task.FromResult(source.ToString(2));
        }
    }
}