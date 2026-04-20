using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using Frank.Mapping.Documents.Extensions;
using Frank.Mapping.Tests.Documents;
using JetBrains.Annotations;

namespace Frank.Mapping.Tests.Extensions;

[TestSubject(typeof(ExpressionExtensions))]
public class ExpressionExtensionsTests : DocumentsTestBase
{
    [Test]
    public async Task GetMemberName_WhenCalledWithPropertyExpression_ReturnsMemberName()
    {
        // Arrange
        Expression<Func<SimpleDocument, string>> expression = x => x.StringProperty;
        
        // Act
        var memberName = expression.GetPropertyInfo().Name;
        
        // Assert
        await Assert.That(memberName).IsEqualTo(nameof(SimpleDocument.StringProperty));
    }
    
    [Test]
    public async Task GetMemberName_WhenCalledWithNestedPropertyExpression_ReturnsMemberName()
    {
        // Arrange
        Expression<Func<SimpleDocument, string>> expression = x => x.NestedDocument.StringProperty;
        
        // Act
        var memberName = expression.GetPropertyInfo().Name;
        
        // Assert
        await Assert.That(memberName).IsEqualTo(nameof(SimpleDocument.NestedDocument.StringProperty));
    }
    
    [Test]
    public async Task GetXPathDefinition_WhenCalledWithPropertyExpression_ReturnsXPathDefinition()
    {
        // Arrange
        Expression<Func<SimpleDocument, string>> expression = x => x.StringProperty;
        
        // Act
        var xPathDefinition = expression.GetXPathDefinition();
        
        // Assert
        await Assert.That(xPathDefinition.Path).IsEqualTo("/StringProperty");
    }
    
    [Test]
    public async Task GetXPathDefinition_WhenCalledWithNestedPropertyExpression_ReturnsXPathDefinition()
    {
        // Arrange
        Expression<Func<SimpleDocument, string>> expression = x => x.NestedDocument.StringProperty;
        
        // Act
        var xPathDefinition = expression.GetXPathDefinition();
        
        // Assert
        await Assert.That(xPathDefinition.Path).IsEqualTo("/NestedDocument/StringProperty");
    }
    
    [Test]
    public async Task GetJsonPathDefinition_WhenCalledWithPropertyExpression_ReturnsJsonPathDefinition()
    {
        // Arrange
        Expression<Func<SimpleDocument, string>> expression = x => x.StringProperty;
        
        // Act
        var jsonPathDefinition = expression.GetJsonPathDefinition();
        
        // Assert
        await Assert.That(jsonPathDefinition.Path).IsEqualTo("$.StringProperty");
    }
    
    [Test]
    public async Task GetJsonPathDefinition_WhenCalledWithNestedPropertyExpression_ReturnsJsonPathDefinition()
    {
        // Arrange
        Expression<Func<SimpleDocument, string>> expression = x => x.NestedDocument.StringProperty;
        
        // Act
        var jsonPathDefinition = expression.GetJsonPathDefinition();
        
        // Assert
        await Assert.That(jsonPathDefinition.Path).IsEqualTo("$.NestedDocument.StringProperty");
    }
    
    [Test]
    public async Task GetConfigPathDefinition_WhenCalledWithPropertyExpression_ReturnsConfigPathDefinition()
    {
        // Arrange
        Expression<Func<SimpleDocument, string>> expression = x => x.StringProperty;
        
        // Act
        var configPathDefinition = expression.GetConfigPathDefinition();
        
        // Assert
        await Assert.That(configPathDefinition.Path).IsEqualTo("StringProperty");
    }
    
    [Test]
    public async Task GetConfigPathDefinition_WhenCalledWithNestedPropertyExpression_ReturnsConfigPathDefinition()
    {
        // Arrange
        Expression<Func<SimpleDocument, string>> expression = x => x.NestedDocument.StringProperty;
        
        // Act
        var configPathDefinition = expression.GetConfigPathDefinition();
        
        // Assert
        await Assert.That(configPathDefinition.Path).IsEqualTo("NestedDocument:StringProperty");
    }
    
    [ExcludeFromCodeCoverage]
    private class SimpleDocument
    {
        public string StringProperty { get; set; }
        
        public NestedDocument NestedDocument { get; set; }
    }
    
    [ExcludeFromCodeCoverage]
    private class NestedDocument
    {
        public List<string> StringList { get; set; }
        
        public string StringProperty { get; set; }
    }
}