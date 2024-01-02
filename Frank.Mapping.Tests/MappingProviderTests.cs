using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Frank.Mapping.Tests;

[TestSubject(typeof(MappingProvider))]
public class MappingProviderTests
{
    private readonly ITestOutputHelper _outputHelper;

    public MappingProviderTests(ITestOutputHelper outputHelper)
    {
        _outputHelper = outputHelper;
    }
    
    [Fact]
    public void Map_ReturnsCorrectResult()
    {
        // Arrange
        var serviceProvider = new ServiceCollection()
            .AddMappingDefinition<From, To, MyMappingDefinition>()
            .BuildServiceProvider();
        var mappingProvider = serviceProvider.GetRequiredService<IMappingProvider>();
        var from = new From
        {
            Id = 1,
            FirstName = "John",
            LastName = "Connor"
        };

        // Act
        var to = mappingProvider.Map<From, To>(from);

        // Assert
        Assert.Equal(from.Id, to.Id);
        Assert.Equal($"{from.FirstName} {from.LastName}", to.Name);
    }
    
    [Fact]
    public void GetMappingDefinition_ReturnsCorrectMappingDefinition()
    {
        // Arrange
        var serviceProvider = new ServiceCollection()
            .AddMappingDefinition<From, To, MyMappingDefinition>()
            .BuildServiceProvider();
        var mappingProvider = serviceProvider.GetRequiredService<IMappingProvider>();

        // Act
        var mappingDefinition = mappingProvider.GetMappingDefinition<From, To>();

        // Assert
        Assert.NotNull(mappingDefinition);
        Assert.IsType<MyMappingDefinition>(mappingDefinition);
    }
    
    [Fact]
    public void GetMappingDefinition_ThrowsExceptionWhenMappingDefinitionNotFound()
    {
        // Arrange
        var serviceProvider = new ServiceCollection()
            .AddMappingDefinition<From, To, MyMappingDefinition>()
            .BuildServiceProvider();
        var mappingProvider = serviceProvider.GetRequiredService<IMappingProvider>();

        // Act
        var exception = Record.Exception(() => mappingProvider.GetMappingDefinition<To, From>());

        // Assert
        Assert.NotNull(exception);
        Assert.IsType<MappingDefinitionNotFoundException>(exception);
    }
    
private class MyMappingDefinition : IMappingDefinition<From, To>
{
    public To Map(From from)
    {
        return new()
        {
            Id = from.Id,
            Name = $"{from.FirstName} {from.LastName}"
        };
    }
}
    
    private class From
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
    }
    
    private class To
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
