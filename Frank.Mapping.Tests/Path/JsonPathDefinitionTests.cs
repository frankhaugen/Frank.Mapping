using Frank.Mapping.Documents.Path;
using Frank.Mapping.Tests.Documents;
using JetBrains.Annotations;
using Xunit.Abstractions;

namespace Frank.Mapping.Tests.Path;

[TestSubject(typeof(JsonPathDefinition))]
public class JsonPathDefinitionTests : DocumentsTestBase
{
    /// <inheritdoc />
    public JsonPathDefinitionTests(ITestOutputHelper outputHelper) : base(outputHelper)
    {
    }
    
    [Fact]
    public void ShouldCreateJsonPathDefinition()
    {
        // Arrange
        const string path = "$.root.element";
        
        // Act
        var definition = new JsonPathDefinition(path);
        
        // Assert
        Assert.Equal(path, definition.Path);
    }
    
    [Fact]
    public void ShouldThrowArgumentExceptionWhenCreatingJsonPathDefinitionWithInvalidPath()
    {
        // Arrange
        const string path = "::SomeInvalidPath";
        
        // Act
        Action act = () => new JsonPathDefinition(path);
        
        // Assert
        var ex = Assert.Throws<ArgumentException>(act);
        Assert.Equal($"The provided JSON path '{path}' is invalid. (Parameter 'path')", ex.Message);
    }
}