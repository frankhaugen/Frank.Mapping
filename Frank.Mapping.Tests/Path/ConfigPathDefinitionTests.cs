using Frank.Mapping.Documents.Path;
using Frank.Mapping.Tests.Documents;
using JetBrains.Annotations;
using Xunit.Abstractions;

namespace Frank.Mapping.Tests.Path;

[TestSubject(typeof(ConfigPathDefinition))]
public class ConfigPathDefinitionTests : DocumentsTestBase
{
    /// <inheritdoc />
    public ConfigPathDefinitionTests(ITestOutputHelper outputHelper) : base(outputHelper)
    {
    }
    
    [Fact]
    public void ShouldCreateConfigPathDefinition()
    {
        // Arrange
        const string path = "root:element";
        
        // Act
        var definition = new ConfigPathDefinition(path);
        
        // Assert
        Assert.Equal(path, definition.Path);
    }
    
    [Fact]
    public void ShouldThrowArgumentExceptionWhenCreatingConfigPathDefinitionWithInvalidPath()
    {
        // Arrange
        const string path = "System.InvalidPath[3].Count";
        
        // Act
        Action act = () => new ConfigPathDefinition(path);
        
        // Assert
        var ex = Assert.Throws<ArgumentException>(act);
        Assert.Equal($"The provided config path '{path}' is invalid. Config paths should not contain dots. (Parameter 'path')", ex.Message);
    }
}