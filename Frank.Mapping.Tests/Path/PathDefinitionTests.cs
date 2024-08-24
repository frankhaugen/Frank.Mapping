using System.Reflection;
using Frank.Mapping.Documents.Path;
using JetBrains.Annotations;
using Xunit.Abstractions;

namespace Frank.Mapping.Tests.Path;

[TestSubject(typeof(PathDefinition))]
public class PathDefinitionTests
{
    private readonly ITestOutputHelper _outputHelper;

    public PathDefinitionTests(ITestOutputHelper outputHelper)
    {
        _outputHelper = outputHelper;
    }
    
    [Fact]
    public void Create_WithValidPath_ReturnsPathDefinition()
    {
        // Arrange
        var path = "path";
        
        // Act
        var result = PathDefinition.Create<MyPathDefinition>(path);
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal(path, result.Path);
    }
    
    [Fact]
    public void Create_WithInvalidPath_ThrowsArgumentException()
    {
        // Arrange
        var path = string.Empty;
        
        // Act
        void Act() => PathDefinition.Create<MyPathDefinition>(path);
        
        // Assert
        Assert.Throws<TargetInvocationException>(Act);
    }
    
    [Fact]
    public void ToString_ReturnsPath()
    {
        // Arrange
        var path = "#";
        var pathDefinition = PathDefinition.Create<MyPathDefinition>(path);
        
        // Act
        var result = pathDefinition?.ToString();
        
        // Assert
        _outputHelper.WriteLine(result);
        Assert.Equal("""MyPathDefinition { Path = # }""", result);
    }
    
    [Fact]
    public void ImplicitOperator_ReturnsPath()
    {
        // Arrange
        var path = "path";
        var pathDefinition = PathDefinition.Create<MyPathDefinition>(path);
        
        // Act
        string result = pathDefinition;
        
        // Assert
        Assert.Equal(path, result);
    }
    
    [Fact]
    public void ImplicitOperator_WithNotNull_ReturnsPath()
    {
        // Arrange
        var path = "MyPathDefinition";
        var pathDefinition = PathDefinition.Create<MyPathDefinition>(path);
        
        // Act
        string? result = pathDefinition;
        
        // Assert
        Assert.Equal(path, result);
    }
    
    record MyPathDefinition : PathDefinition
    {
        public MyPathDefinition(string path) : base(path)
        {
        }
    }
}