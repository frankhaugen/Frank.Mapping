using Frank.Mapping.Documents.Path;
using Frank.Mapping.Tests.Documents;
using JetBrains.Annotations;

namespace Frank.Mapping.Tests.Path;

[TestSubject(typeof(ConfigPathDefinition))]
public class ConfigPathDefinitionTests : DocumentsTestBase
{
    [Test]
    public async Task ShouldCreateConfigPathDefinition()
    {
        // Arrange
        const string path = "root:element";
        
        // Act
        var definition = new ConfigPathDefinition(path);
        
        // Assert
        await Assert.That(definition.Path).IsEqualTo(path);
    }
    
    [Test]
    public async Task ShouldThrowArgumentExceptionWhenCreatingConfigPathDefinitionWithInvalidPath()
    {
        // Arrange
        const string path = "System.InvalidPath[3].Count";
        
        // Act
        Action act = () => new ConfigPathDefinition(path);
        
        // Assert
        var ex = await Assert.That(act).ThrowsExactly<ArgumentException>();
        await Assert.That(ex!.Message).IsEqualTo($"The provided config path '{path}' is invalid. Config paths should not contain dots. (Parameter 'path')");
    }
}