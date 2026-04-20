using Frank.Mapping.Documents.Helpers;
using JetBrains.Annotations;

namespace Frank.Mapping.Tests.Helpers;

[TestSubject(typeof(JsonPathHelper))]
public class JsonPathHelperTests
{
    [Test]
    public async Task ShouldGetJsonPath()
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
        await Assert.That(result).IsNotNull();
        Console.WriteLine($"Test Result:");
        Console.WriteLine(string.Join(Environment.NewLine, result));
    }
    
    [Test]
    public async Task ShouldThrowException_WhenJsonPathIsNotValid()
    {
        // Arrange
        var jsonPath = "Name";
        
        // Act & Assert
        await Assert.That(() => JsonPathHelper.GetPaths(jsonPath)).ThrowsExactly<ArgumentException>();
    }
    
    [Test]
    public async Task ShouldThrowException_WhenJsonPathIsNotJsonPath()
    {
        // Arrange
        var jsonPath = "<xml></xml>";
        
        // Act & Assert
        await Assert.That(() => JsonPathHelper.GetPaths(jsonPath)).ThrowsExactly<ArgumentException>();
    }
}