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
        Assert.NotNull(mappingProvider);
        
        var result = mappingProvider.Map<Version, string>(new Version(1, 2, 3, 4));
        _outputHelper.WriteLine(result);
        Assert.Equal("1.2", result);
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
    
}