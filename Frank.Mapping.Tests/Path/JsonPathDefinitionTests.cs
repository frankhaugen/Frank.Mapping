using Frank.Mapping.Documents.Path;
using Frank.Mapping.Tests.Documents;
using JetBrains.Annotations;

namespace Frank.Mapping.Tests.Path;

[TestSubject(typeof(JsonPathDefinition))]
public class JsonPathDefinitionTests : DocumentsTestBase
{
    [Test]
    public async Task ShouldCreateJsonPathDefinition()
    {
        // Arrange
        const string path = "$.root.element";
        
        // Act
        var definition = new JsonPathDefinition(path);
        
        // Assert
        await Assert.That(definition.Path).IsEqualTo(path);
    }
    
    [Test]
    public async Task ShouldThrowArgumentExceptionWhenCreatingJsonPathDefinitionWithInvalidPath()
    {
        // Arrange
        const string path = "::SomeInvalidPath";
        
        // Act
        Action act = () => new JsonPathDefinition(path);
        
        // Assert
        var ex = await Assert.That(act).ThrowsExactly<ArgumentException>();
        await Assert.That(ex!.Message).IsEqualTo($"The provided JSON path '{path}' is invalid. (Parameter 'path')");
    }
}