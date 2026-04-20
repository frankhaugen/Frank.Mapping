using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
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
        Assert.Equal(nameof(SimpleDocument.StringProperty), memberName);
    }
    
    [Fact]
    public void GetMemberName_WhenCalledWithNestedPropertyExpression_ReturnsMemberName()
    {
        // Arrange
        Expression<Func<SimpleDocument, string>> expression = x => x.NestedDocument.StringProperty;
        
        // Act
        var memberName = expression.GetPropertyInfo().Name;
        
        // Assert
        Assert.Equal(nameof(SimpleDocument.NestedDocument.StringProperty), memberName);
    }
    
    [Fact]
    public void GetXPathDefinition_WhenCalledWithPropertyExpression_ReturnsXPathDefinition()
    {
        // Arrange
        Expression<Func<SimpleDocument, string>> expression = x => x.StringProperty;
        
        // Act
        var xPathDefinition = expression.GetXPathDefinition();
        
        // Assert
        Assert.Equal("/StringProperty", xPathDefinition.Path);
    }
    
    [Fact]
    public void GetXPathDefinition_WhenCalledWithNestedPropertyExpression_ReturnsXPathDefinition()
    {
        // Arrange
        Expression<Func<SimpleDocument, string>> expression = x => x.NestedDocument.StringProperty;
        
        // Act
        var xPathDefinition = expression.GetXPathDefinition();
        
        // Assert
        Assert.Equal("/NestedDocument/StringProperty", xPathDefinition.Path);
    }
    
    [Fact]
    public void GetJsonPathDefinition_WhenCalledWithPropertyExpression_ReturnsJsonPathDefinition()
    {
        // Arrange
        Expression<Func<SimpleDocument, string>> expression = x => x.StringProperty;
        
        // Act
        var jsonPathDefinition = expression.GetJsonPathDefinition();
        
        // Assert
        Assert.Equal("$.StringProperty", jsonPathDefinition.Path);
    }
    
    [Fact]
    public void GetJsonPathDefinition_WhenCalledWithNestedPropertyExpression_ReturnsJsonPathDefinition()
    {
        // Arrange
        Expression<Func<SimpleDocument, string>> expression = x => x.NestedDocument.StringProperty;
        
        // Act
        var jsonPathDefinition = expression.GetJsonPathDefinition();
        
        // Assert
        Assert.Equal("$.NestedDocument.StringProperty", jsonPathDefinition.Path);
    }
    
    [Fact]
    public void GetConfigPathDefinition_WhenCalledWithPropertyExpression_ReturnsConfigPathDefinition()
    {
        // Arrange
        Expression<Func<SimpleDocument, string>> expression = x => x.StringProperty;
        
        // Act
        var configPathDefinition = expression.GetConfigPathDefinition();
        
        // Assert
        Assert.Equal("StringProperty", configPathDefinition.Path);
    }
    
    [Fact]
    public void GetConfigPathDefinition_WhenCalledWithNestedPropertyExpression_ReturnsConfigPathDefinition()
    {
        // Arrange
        Expression<Func<SimpleDocument, string>> expression = x => x.NestedDocument.StringProperty;
        
        // Act
        var configPathDefinition = expression.GetConfigPathDefinition();
        
        // Assert
        Assert.Equal("NestedDocument:StringProperty", configPathDefinition.Path);
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
