using System.Linq.Expressions;
using FluentAssertions;
using Frank.Mapping.Documents.Extensions;
using Frank.Mapping.Tests.Documents;
using JetBrains.Annotations;
using Xunit.Abstractions;

namespace Frank.Mapping.Tests.Extensions;

[TestSubject(typeof(ExpressionExtensions))]
public class ExpressionExtensionsTests : DocumentsTestBase
{
    /// <inheritdoc />
    public ExpressionExtensionsTests(ITestOutputHelper outputHelper) : base(outputHelper)
    {
    }
    
    [Fact]
    public void GetMemberName_WhenCalledWithPropertyExpression_ReturnsMemberName()
    {
        // Arrange
        Expression<Func<SimpleDocument, string>> expression = x => x.StringProperty;
        
        // Act
        var memberName = expression.GetPropertyInfo().Name;
        
        // Assert
        memberName.Should().Be(nameof(SimpleDocument.StringProperty));
    }
    
    [Fact]
    public void GetMemberName_WhenCalledWithNestedPropertyExpression_ReturnsMemberName()
    {
        // Arrange
        Expression<Func<SimpleDocument, string>> expression = x => x.NestedDocument.StringProperty;
        
        // Act
        var memberName = expression.GetPropertyInfo().Name;
        
        // Assert
        memberName.Should().Be(nameof(SimpleDocument.NestedDocument.StringProperty));
    }
    
    [Fact]
    public void GetXPathDefinition_WhenCalledWithPropertyExpression_ReturnsXPathDefinition()
    {
        // Arrange
        Expression<Func<SimpleDocument, string>> expression = x => x.StringProperty;
        
        // Act
        var xPathDefinition = expression.GetXPathDefinition();
        
        // Assert
        xPathDefinition.Path.Should().Be("/StringProperty");
    }
    
    [Fact]
    public void GetXPathDefinition_WhenCalledWithNestedPropertyExpression_ReturnsXPathDefinition()
    {
        // Arrange
        Expression<Func<SimpleDocument, string>> expression = x => x.NestedDocument.StringProperty;
        
        // Act
        var xPathDefinition = expression.GetXPathDefinition();
        
        // Assert
        xPathDefinition.Path.Should().Be("/NestedDocument/StringProperty");
    }
    
    [Fact]
    public void GetJsonPathDefinition_WhenCalledWithPropertyExpression_ReturnsJsonPathDefinition()
    {
        // Arrange
        Expression<Func<SimpleDocument, string>> expression = x => x.StringProperty;
        
        // Act
        var jsonPathDefinition = expression.GetJsonPathDefinition();
        
        // Assert
        jsonPathDefinition.Path.Should().Be("$.StringProperty");
    }
    
    [Fact]
    public void GetJsonPathDefinition_WhenCalledWithNestedPropertyExpression_ReturnsJsonPathDefinition()
    {
        // Arrange
        Expression<Func<SimpleDocument, string>> expression = x => x.NestedDocument.StringProperty;
        
        // Act
        var jsonPathDefinition = expression.GetJsonPathDefinition();
        
        // Assert
        jsonPathDefinition.Path.Should().Be("$.NestedDocument.StringProperty");
    }
    
    [Fact]
    public void GetConfigPathDefinition_WhenCalledWithPropertyExpression_ReturnsConfigPathDefinition()
    {
        // Arrange
        Expression<Func<SimpleDocument, string>> expression = x => x.StringProperty;
        
        // Act
        var configPathDefinition = expression.GetConfigPathDefinition();
        
        // Assert
        configPathDefinition.Path.Should().Be("StringProperty");
    }
    
    [Fact]
    public void GetConfigPathDefinition_WhenCalledWithNestedPropertyExpression_ReturnsConfigPathDefinition()
    {
        // Arrange
        Expression<Func<SimpleDocument, string>> expression = x => x.NestedDocument.StringProperty;
        
        // Act
        var configPathDefinition = expression.GetConfigPathDefinition();
        
        // Assert
        configPathDefinition.Path.Should().Be("NestedDocument:StringProperty");
    }
    
    private class SimpleDocument
    {
        public string StringProperty { get; set; }
        
        public NestedDocument NestedDocument { get; set; }
    }
    
    private class NestedDocument
    {
        public List<string> StringList { get; set; }
        
        public string StringProperty { get; set; }
    }
}
