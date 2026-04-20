using Frank.Mapping.Documents.Helpers;
using Frank.Mapping.Documents.Models.Enums;
using JetBrains.Annotations;

namespace Frank.Mapping.Tests.Helpers;

[TestSubject(typeof(DocumentVariantHelper))]
public class DocumentVariantHelperTests
{
    [Test]
    public async Task ShouldGetDocumentVariant()
    {
        // Arrange
        var document = "<xml></xml>";
        
        // Act
        var result = DocumentVariantHelper.GetDocumentVariant(document);
        
        // Assert
        await Assert.That(result).IsEqualTo(DocumentVariant.Xml);
        Console.WriteLine($"Test Result: {result}");
    }
    
    [Test]
    public async Task ShouldGetDocumentVariant_WhenJson()
    {
        // Arrange
        var document = "{\"key\": \"value\"}";
        
        // Act
        var result = DocumentVariantHelper.GetDocumentVariant(document);
        
        // Assert
        await Assert.That(result).IsEqualTo(DocumentVariant.Json);
        Console.WriteLine($"Test Result: {result}");
    }
    
    [Test]
    public async Task ShouldThrowException_WhenDocumentVariantNotRecognized()
    {
        // Arrange
        var document = "document";
        
        // Act & Assert
        await Assert.That(() => DocumentVariantHelper.GetDocumentVariant(document)).ThrowsExactly<ArgumentException>();
    }
}