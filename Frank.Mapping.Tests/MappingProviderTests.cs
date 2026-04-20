using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace Frank.Mapping.Tests;

[TestSubject(typeof(MappingProvider))]
public class MappingProviderTests
{
    [Test]
    public async Task Map_ReturnsCorrectResult()
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
        await Assert.That(to.Id).IsEqualTo(from.Id);
        await Assert.That(to.Name).IsEqualTo($"{from.FirstName} {from.LastName}");
    }
    
    [Test]
    public async Task GetMappingDefinition_ReturnsCorrectMappingDefinition()
    {
        // Arrange
        var serviceProvider = new ServiceCollection()
            .AddMappingDefinition<From, To, MyMappingDefinition>()
            .BuildServiceProvider();
        var mappingProvider = serviceProvider.GetRequiredService<IMappingProvider>();

        // Act
        var mappingDefinition = mappingProvider.GetMappingDefinition<From, To>();

        // Assert
        await Assert.That(mappingDefinition).IsNotNull();
        await Assert.That(mappingDefinition).IsAssignableTo<MyMappingDefinition>();
    }
    
    [Test]
    public async Task GetMappingDefinition_ThrowsExceptionWhenMappingDefinitionNotFound()
    {
        // Arrange
        var serviceProvider = new ServiceCollection()
            .AddMappingDefinition<From, To, MyMappingDefinition>()
            .BuildServiceProvider();
        var mappingProvider = serviceProvider.GetRequiredService<IMappingProvider>();

        // Act & Assert
        await Assert.That(() => mappingProvider.GetMappingDefinition<To, From>()).ThrowsExactly<MappingDefinitionNotFoundException>();
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