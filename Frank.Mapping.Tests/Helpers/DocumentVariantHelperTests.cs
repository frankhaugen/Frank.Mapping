using FluentAssertions;
using Frank.Mapping.Documents.Helpers;
using Frank.Mapping.Documents.Models.Enums;
using JetBrains.Annotations;
using Xunit.Abstractions;

namespace Frank.Mapping.Tests.Helpers;

[TestSubject(typeof(DocumentVariantHelper))]
public class DocumentVariantHelperTests
{
    private readonly ITestOutputHelper _output;

    public DocumentVariantHelperTests(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public void ShouldGetDocumentVariant()
    {
        // Arrange
        var document = "<xml></xml>";
        
        // Act
        var result = DocumentVariantHelper.GetDocumentVariant(document);
        
        // Assert
        result.Should().Be(DocumentVariant.Xml);
        _output.WriteLine($"Test Result: {result}");
    }
    
    [Fact]
    public void ShouldGetDocumentVariant_WhenJson()
    {
        // Arrange
        var document = "{\"key\": \"value\"}";
        
        // Act
        var result = DocumentVariantHelper.GetDocumentVariant(document);
        
        // Assert
        result.Should().Be(DocumentVariant.Json);
        _output.WriteLine($"Test Result: {result}");
    }
    
    [Fact]
    public void ShouldThrowException_WhenDocumentVariantNotRecognized()
    {
        // Arrange
        var document = "document";
        
        // Act & Assert
        Assert.Throws<ArgumentException>(() => DocumentVariantHelper.GetDocumentVariant(document));
    }
}