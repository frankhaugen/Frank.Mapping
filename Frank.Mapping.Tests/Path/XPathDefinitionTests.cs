using FluentAssertions;
using Frank.Mapping.Documents.Path;
using Frank.Mapping.Tests.Documents;
using JetBrains.Annotations;
using Xunit.Abstractions;

namespace Frank.Mapping.Tests.Path;

[TestSubject(typeof(XPathDefinition))]
public class XPathDefinitionTest : DocumentsTestBase
{
    /// <inheritdoc />
    public XPathDefinitionTest(ITestOutputHelper outputHelper) : base(outputHelper)
    {
    }
    
    [Fact]
    public void ShouldCreateXPathDefinition()
    {
        // Arrange
        const string path = "/root/element";
        
        // Act
        var definition = new XPathDefinition(path);
        
        // Assert
        definition.Path.Should().Be(path);
    }
    
    [Fact]
    public void ShouldThrowArgumentExceptionWhenCreatingXPathDefinitionWithInvalidPath()
    {
        // Arrange
        const string path = "::SomeInvalidPath";
        
        // Act
        Action act = () => new XPathDefinition(path);
        
        // Assert
        act.Should().Throw<ArgumentException>().WithMessage($"The provided XPath '{path}' is invalid. (Parameter 'path')");
    }
}