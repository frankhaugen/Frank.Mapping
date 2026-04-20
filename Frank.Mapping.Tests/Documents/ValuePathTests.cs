using Frank.Mapping.Documents;
using Frank.Mapping.Documents.Models;
using Frank.Mapping.Documents.Models.Enums;
using Xunit.Abstractions;

namespace Frank.Mapping.Tests.Documents;

public class ValuePathTests : DocumentsTestBase
{
    /// <inheritdoc />
    public ValuePathTests(ITestOutputHelper outputHelper) : base(outputHelper)
    {
    }

    [Fact]
    public void Constructor_WithDocumentVariantAndPathAndType_SetsProperties()
    {
        // Arrange
        var documentVariant = DocumentVariant.Json;
        var path = "$.Name";
        var type = typeof(string);
        var jsonDocument = _jsonDocument;

        // Act
        var valuePath = new ValuePath<string>(documentVariant, path);

        // Assert
        Assert.Equal(documentVariant, valuePath.DocumentVariant);
        Assert.Equal(path, valuePath.Path);
        Assert.Equal(type, valuePath.ValueType);
        Assert.Equal("John Doe", valuePath.GetValue(jsonDocument));
    }
    
    [Fact]
    public void Constructor_WithDocumentVariantAndPathAndType_ThrowsWhenDocumentVariantIsNull()
    {
        // Arrange
        var documentVariant = DocumentVariant.Json;
        var path = "$.Name";
        var type = typeof(string);
        var jsonDocument = _jsonDocument;

        // Act
        var valuePath = new ValuePath(documentVariant, path, type);

        // Assert
        Assert.Equal(documentVariant, valuePath.DocumentVariant);
        Assert.Equal(path, valuePath.Path);
        Assert.Equal(type, valuePath.ValueType);
        Assert.Equal("John Doe", valuePath.GetValue(jsonDocument));
    }
}