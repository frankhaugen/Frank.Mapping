using FluentAssertions;
using Frank.Mapping.Documents;
using Frank.Mapping.Documents.Models;
using Frank.Mapping.Documents.Models.Enums;
using Xunit.Abstractions;

namespace Frank.Mapping.Tests.Documents;

public class DocumentValuesExtractorTests : DocumentsTestBase
{
    /// <inheritdoc />
    public DocumentValuesExtractorTests(ITestOutputHelper outputHelper) : base(outputHelper)
    {
    }

    [Fact]
    public void ExtractValuesFromDocument()
    {
        // Arrange
        var valuePaths = new List<ValuePath>
        {
            new ValuePath<string>(DocumentVariant.Json, "$.Name"),
            new ValuePath<int>(DocumentVariant.Json, "$.age"),
            new ValuePath<string>(DocumentVariant.Json, "$.Address.City"),
            new ValuePath<string>(DocumentVariant.Json, "$.Address.Street")
        };
        var documentValuesExtractor = new DocumentValuesExtractor(valuePaths);
        var jsonDocument = _jsonDocument;

        // Act
        var values = documentValuesExtractor.ExtractValuesFromDocument(jsonDocument).ToList();

        // Assert
        values.Should().NotBeNull();
        values.Should().HaveCount(4);
        
        _outputHelper.WriteLine(values.Select(x => x.ToString()));
    }
    
    [Fact]
    public void ExtractValuesFromXmlDocument()
    {
        // Arrange
        var valuePaths = new List<ValuePath>
        {
            new ValuePath<string>(DocumentVariant.Xml, "/Person/Name"),
            new ValuePath<int>(DocumentVariant.Xml, "/Person/age"),
            new ValuePath<string>(DocumentVariant.Xml, "/Person/Address/City"),
            new ValuePath<string>(DocumentVariant.Xml, "/Person/Address/Street")
        };
        var documentValuesExtractor = new DocumentValuesExtractor(valuePaths);
        var xmlDocument = _xmlDocument;

        // Act
        var values = documentValuesExtractor.ExtractValuesFromDocument(xmlDocument).ToList();

        // Assert
        values.Should().NotBeNull();
        values.Should().HaveCount(4);
        
        _outputHelper.WriteLine(values.Select(x => x.ToString()));
    }
}