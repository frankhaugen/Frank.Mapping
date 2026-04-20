using Frank.Mapping.Documents;
using Frank.Mapping.Documents.Models;
using Frank.Mapping.Documents.Models.Enums;

namespace Frank.Mapping.Tests.Documents;

public class ValuePathTests : DocumentsTestBase
{
    [Test]
    public async Task Constructor_WithDocumentVariantAndPathAndType_SetsProperties()
    {
        // Arrange
        var documentVariant = DocumentVariant.Json;
        var path = "$.Name";
        var type = typeof(string);
        var jsonDocument = _jsonDocument;

        // Act
        var valuePath = new ValuePath<string>(documentVariant, path);

        // Assert
        await Assert.That(valuePath.DocumentVariant).IsEqualTo(documentVariant);
        await Assert.That(valuePath.Path).IsEqualTo(path);
        await Assert.That(valuePath.ValueType).IsEqualTo(type);
        await Assert.That(valuePath.GetValue(jsonDocument)).IsEqualTo("John Doe");
    }
    
    [Test]
    public async Task Constructor_WithDocumentVariantAndPathAndType_ThrowsWhenDocumentVariantIsNull()
    {
        // Arrange
        var documentVariant = DocumentVariant.Json;
        var path = "$.Name";
        var type = typeof(string);
        var jsonDocument = _jsonDocument;

        // Act
        var valuePath = new ValuePath(documentVariant, path, type);

        // Assert
        await Assert.That(valuePath.DocumentVariant).IsEqualTo(documentVariant);
        await Assert.That(valuePath.Path).IsEqualTo(path);
        await Assert.That(valuePath.ValueType).IsEqualTo(type);
        await Assert.That(valuePath.GetValue(jsonDocument)).IsEqualTo("John Doe");
    }
}