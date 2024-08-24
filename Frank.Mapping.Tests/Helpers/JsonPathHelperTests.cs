using FluentAssertions;
using Frank.Mapping.Documents.Helpers;
using JetBrains.Annotations;
using Xunit.Abstractions;

namespace Frank.Mapping.Tests.Helpers;

[TestSubject(typeof(JsonPathHelper))]
public class JsonPathHelperTests
{
    private readonly ITestOutputHelper _outputHelper;

    public JsonPathHelperTests(ITestOutputHelper outputHelper)
    {
        _outputHelper = outputHelper;
    }
    
    [Fact]
    public void ShouldGetJsonPath()
    {
        // Arrange
        var jsonPath = 
            """
            {
                "key": "value",
                "nested": {
                    "key1": "value1"
                },
                "array": [
                    {
                        "key2": "value2"
                    }
                ]
            }
            """;
        
        // Act
        var result = JsonPathHelper.GetPaths(jsonPath).ToArray();
        
        // Assert
        result.Should().NotBeNull();
        _outputHelper.WriteLine($"Test Result:");
        _outputHelper.WriteLine(result);
    }
    
    [Fact]
    public void ShouldThrowException_WhenJsonPathIsNotValid()
    {
        // Arrange
        var jsonPath = "Name";
        
        // Act & Assert
        Assert.Throws<ArgumentException>(() => JsonPathHelper.GetPaths(jsonPath));
    }
    
    [Fact]
    public void ShouldThrowException_WhenJsonPathIsNotJsonPath()
    {
        // Arrange
        var jsonPath = "<xml></xml>";
        
        // Act & Assert
        Assert.Throws<ArgumentException>(() => JsonPathHelper.GetPaths(jsonPath));
    }
}