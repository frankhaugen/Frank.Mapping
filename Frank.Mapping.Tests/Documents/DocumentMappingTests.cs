using Frank.Mapping.Documents;
using Frank.Mapping.Documents.Models;
using Frank.Mapping.Documents.Models.Enums;

namespace Frank.Mapping.Tests.Documents;

public class DocumentMappingTests : DocumentsTestBase
{
    [Test]
    public async Task Test1()
    {
        // Arrange
        var documentVariant = DocumentVariant.Json;
        var propperyMappings = new List<PropertyMapping>
        {
            new PropertyMapping<Person, string>(person => person.Name, new ValuePath<string>(documentVariant, "$.Name")),
            new PropertyMapping<Person, int>(person => person.Age, new ValuePath<int>(documentVariant, "$.age")),
            new PropertyMapping<Person, Address>(person => person.Address, new ValuePath<Address>(documentVariant, "$.Address")),
            new PropertyMapping<Person, string>(person => person.Address.City, new ValuePath<string>(documentVariant, "$.Address.City")),
            new PropertyMapping<Person, string?>(person => person.Address.Street, new ValuePath<string>(documentVariant, "$.Address.Street"))
        };
        
        var jsonDocument = _jsonDocument;
        
        // Act
        var action = new Action(() => new DocumentMapping<Person>(documentVariant, propperyMappings));
        
        // Assert
        await Assert.That(action).ThrowsNothing();
    }
}