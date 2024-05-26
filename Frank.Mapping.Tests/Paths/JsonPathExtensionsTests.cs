using System.Linq.Expressions;
using System.Text.Json;
using System.Text.Json.Nodes;
using FluentAssertions;
using Frank.Mapping.Documents;
using Frank.Mapping.Documents.Models;
using Xunit.Abstractions;

namespace Frank.Mapping.Tests.Paths;

public class JsonPathExtensionsTests
{
    private readonly ITestOutputHelper _output;

    public JsonPathExtensionsTests(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public void ShouldConvertSimplePropertyExpressionToJsonPath()
    {
        // Arrange
        Expression<Func<SampleClass, object>> expression = x => x.Name;
        
        // Act
        var jsonPath = expression.GetPathDefinition();
        
        // Assert
        jsonPath.Path.Should().Be("$.Name");
        _output.WriteLine($"Test Result: {jsonPath.Path}");
    }

    [Fact]
    public void ShouldConvertAnotherSimplePropertyExpressionToJsonPath()
    {
        // Arrange
        Expression<Func<SampleClass, int>> expression = x => x.Age;
        
        // Act
        var jsonPath = expression.GetPathDefinition();
        
        // Assert
        jsonPath.Path.Should().Be("$.Age");
        _output.WriteLine($"Test Result: {jsonPath.Path}");
    }
    
    [Fact]
    public void ShouldConvertNestedPropertyExpressionToJsonPath()
    {
        // Arrange
        Expression<Func<SampleClass, string?>> expression = x => x.Address.City;
        
        // Act
        var jsonPath = expression.GetPathDefinition();
        
        // Assert
        jsonPath.Path.Should().Be("$.Address.City");
        _output.WriteLine($"Test Result: {jsonPath.Path}");
    }
    
    [Fact]
    public void ShouldConvertNestedIndexerExpressionToJsonPath()
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
        var jsonPath = expression.GetJsonPath();
        var evalResult = jsonPath.Evaluate(jsonRoot);
        var result = evalResult.Matches!.Select(x => x.Value!.GetValue<string>()).First();
        
        // Assert
        _output.WriteLine($"Expression: {expression}");
        _output.WriteLine($"Test Result: {jsonPath}");
        _output.WriteLine(result);
        jsonPath.ToString().Should().Be("$.Addresses[0].City");
        result.Should().Be("New York");
    }

    // Sample class for testing
    private class SampleClass
    {
        public string Name { get; set; }
        public int Age { get; set; }
        
        public Address Address { get; set; }
        
        public Address[] Addresses { get; set; } = [];
    }

    private class Address
    {
        public string Street { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
    }
}
