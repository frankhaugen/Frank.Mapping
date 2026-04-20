using Frank.Mapping.Documents;
using Frank.Mapping.Documents.Models;
using Frank.Mapping.Documents.Models.Enums;

namespace Frank.Mapping.Tests.Documents;

public class DocumentValuesExtractorTests : DocumentsTestBase
{
    [Test]
    public async Task ExtractValuesFromDocument()
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
        await Assert.That(values).IsNotNull();
        await Assert.That(values.Count).IsEqualTo(4);
        
        Console.WriteLine(string.Join(Environment.NewLine, values.Select(x => x.ToString())));
    }
    
    [Test]
    public async Task ExtractValuesFromXmlDocument()
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
        await Assert.That(values).IsNotNull();
        await Assert.That(values.Count).IsEqualTo(4);
        
        Console.WriteLine(string.Join(Environment.NewLine, values.Select(x => x.ToString())));
    }
}