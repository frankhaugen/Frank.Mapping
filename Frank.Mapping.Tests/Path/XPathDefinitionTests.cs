using Frank.Mapping.Documents.Path;
using Frank.Mapping.Tests.Documents;
using JetBrains.Annotations;

namespace Frank.Mapping.Tests.Path;

[TestSubject(typeof(XPathDefinition))]
public class XPathDefinitionTest : DocumentsTestBase
{
    [Test]
    public async Task ShouldCreateXPathDefinition()
    {
        // Arrange
        const string path = "/root/element";
        
        // Act
        var definition = new XPathDefinition(path);
        
        // Assert
        await Assert.That(definition.Path).IsEqualTo(path);
    }
    
    [Test]
    public async Task ShouldThrowArgumentExceptionWhenCreatingXPathDefinitionWithInvalidPath()
    {
        // Arrange
        const string path = "::SomeInvalidPath";
        
        // Act
        Action act = () => new XPathDefinition(path);
        
        // Assert
        var ex = await Assert.That(act).ThrowsExactly<ArgumentException>();
        await Assert.That(ex!.Message).IsEqualTo($"The provided XPath '{path}' is invalid. (Parameter 'path')");
    }
}