using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Text.Json;
using System.Text.Json.Nodes;
using Frank.Mapping.Documents;
using Frank.Mapping.Documents.Extensions;
using Frank.Mapping.Documents.Models;

namespace Frank.Mapping.Tests.Paths;

public class JsonPathExtensionsTests
{
    [Test]
    public async Task ShouldConvertSimplePropertyExpressionToJsonPath()
    {
        // Arrange
        Expression<Func<SampleClass, object>> expression = x => x.Name;
        
        // Act
        var jsonPath = expression.GetJsonPathDefinition();
        
        // Assert
        await Assert.That(jsonPath.Path).IsEqualTo("$.Name");
        Console.WriteLine($"Test Result: {jsonPath.Path}");
    }

    [Test]
    public async Task ShouldConvertAnotherSimplePropertyExpressionToJsonPath()
    {
        // Arrange
        Expression<Func<SampleClass, int>> expression = x => x.Age;
        
        // Act
        var jsonPath = expression.GetJsonPathDefinition();
        
        // Assert
        await Assert.That(jsonPath.Path).IsEqualTo("$.Age");
        Console.WriteLine($"Test Result: {jsonPath.Path}");
    }
    
    [Test]
    public async Task ShouldConvertNestedPropertyExpressionToJsonPath()
    {
        // Arrange
        Expression<Func<SampleClass, string?>> expression = x => x.Address.City;
        
        // Act
        var jsonPath = expression.GetJsonPathDefinition();
        
        // Assert
        await Assert.That(jsonPath.Path).IsEqualTo("$.Address.City");
        Console.WriteLine($"Test Result: {jsonPath.Path}");
    }
    
    [Test]
    public async Task ShouldConvertNestedIndexerExpressionToJsonPath()
    {
        // Arrange
        Expression<Func<SampleClass, string?>> expression = x => x.Addresses[0].City;
        
        var model = new SampleClass
        {
            Addresses = new[]
            {
                new Address
                {
                    City = "New York"
                }
            }
        };
        var jsonRoot = JsonNode.Parse(JsonSerializer.SerializeToUtf8Bytes(model));
        
        // Act
        var jsonPath = expression.GetJsonPathDefinition().JsonPath;
        var evalResult = jsonPath.Evaluate(jsonRoot);
        var result = evalResult.Matches!.Select(x => x.Value!.GetValue<string>()).First();
        
        // Assert
        Console.WriteLine($"Expression: {expression}");
        Console.WriteLine($"Test Result: {jsonPath}");
        Console.WriteLine(result);
        await Assert.That(jsonPath.ToString()).IsEqualTo("$.Addresses[0].City");
        await Assert.That(result).IsEqualTo("New York");
    }

    // Sample class for testing
    [ExcludeFromCodeCoverage]
    private class SampleClass
    {
        public string Name { get; set; }
        public int Age { get; set; }
        
        public Address Address { get; set; }
        
        public Address[] Addresses { get; set; } = [];
    }

    [ExcludeFromCodeCoverage]
    private class Address
    {
        public string Street { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
    }
}