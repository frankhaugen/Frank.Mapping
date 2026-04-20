using System.Reflection;
using Frank.Mapping.Documents.Path;
using JetBrains.Annotations;

namespace Frank.Mapping.Tests.Path;

[TestSubject(typeof(PathDefinition))]
public class PathDefinitionTests
{
    [Test]
    public async Task Create_WithValidPath_ReturnsPathDefinition()
    {
        // Arrange
        var path = "path";
        
        // Act
        var result = PathDefinition.Create<MyPathDefinition>(path);
        
        // Assert
        await Assert.That(result).IsNotNull();
        await Assert.That(result!.Path).IsEqualTo(path);
    }
    
    [Test]
    public async Task Create_WithInvalidPath_ThrowsArgumentException()
    {
        // Arrange
        var path = string.Empty;
        
        // Act
        void Act() => PathDefinition.Create<MyPathDefinition>(path);
        
        // Assert
        await Assert.That(Act).ThrowsExactly<TargetInvocationException>();
    }
    
    [Test]
    public async Task ToString_ReturnsPath()
    {
        // Arrange
        var path = "#";
        var pathDefinition = PathDefinition.Create<MyPathDefinition>(path);
        
        // Act
        var result = pathDefinition?.ToString();
        
        // Assert
        Console.WriteLine(result);
        await Assert.That(result).IsEqualTo("MyPathDefinition { Path = # }");
    }
    
    [Test]
    public async Task ImplicitOperator_ReturnsPath()
    {
        // Arrange
        var path = "path";
        var pathDefinition = PathDefinition.Create<MyPathDefinition>(path);
        
        // Act
        string result = pathDefinition!;
        
        // Assert
        await Assert.That(result).IsEqualTo(path);
    }
    
    [Test]
    public async Task ImplicitOperator_WithNotNull_ReturnsPath()
    {
        // Arrange
        var path = "MyPathDefinition";
        var pathDefinition = PathDefinition.Create<MyPathDefinition>(path);
        
        // Act
        string? result = pathDefinition;
        
        // Assert
        await Assert.That(result).IsEqualTo(path);
    }
    
    record MyPathDefinition : PathDefinition
    {
        public MyPathDefinition(string path) : base(path)
        {
        }
    }
}