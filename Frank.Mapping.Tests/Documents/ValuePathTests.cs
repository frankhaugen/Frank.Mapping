using FluentAssertions;
using Frank.Mapping.Documents;
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
        valuePath.DocumentVariant.Should().Be(documentVariant);
        valuePath.Path.Should().Be(path);
        valuePath.ValueType.Should().Be(type);
        valuePath.GetValue(jsonDocument).Should().Be("John Doe");
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
        valuePath.DocumentVariant.Should().Be(documentVariant);
        valuePath.Path.Should().Be(path);
        valuePath.ValueType.Should().Be(type);
        valuePath.GetValue(jsonDocument).Should().Be("John Doe");
    }
}