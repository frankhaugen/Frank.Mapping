using FluentAssertions;
using Frank.Mapping.Documents;
using Frank.Mapping.Documents.Models;
using Frank.Mapping.Documents.Models.Enums;
using Xunit.Abstractions;

namespace Frank.Mapping.Tests.Documents;

public class DocumentTests : DocumentsTestBase
{
    /// <inheritdoc />
    public DocumentTests(ITestOutputHelper outputHelper) : base(outputHelper)
    {
    }

    [Fact]
    public void Test()
    {
        var document = new Document(_xmlDocument);
        var documentMapping = new DocumentMapping<Person>(DocumentVariant.Xml, new List<PropertyMapping>()
        {
            new PropertyMapping<Person, string>(d => d.Name, new ValuePath(DocumentVariant.Xml, "/Person/Name", typeof(string))),
            new PropertyMapping<Person, int>(d => d.Age, new ValuePath(DocumentVariant.Xml, "/Person/age", typeof(int))),
        });

        var person = new Person()
        {
            Name = "Hurdy Gurdy",
            Age = 69,
        };
        
        document.MapTo<Person>(person, documentMapping);

        person.Name.Should().Be("John Doe");
        person.Age.Should().Be(30);
    }
    
    [Fact]
    public void Test2()
    {
        var document = new Document(_jsonDocument);
        var documentMapping = new DocumentMapping<Person>(DocumentVariant.Json, new List<PropertyMapping>()
        {
            new PropertyMapping<Person, string>(d => d.Name, new ValuePath(DocumentVariant.Json, "$.Name", typeof(string))),
            new PropertyMapping<Person, int>(d => d.Age, new ValuePath(DocumentVariant.Json, "$.age", typeof(int))),
        });

        var person = new Person()
        {
            Name = "Hurdy Gurdy",
            Age = 69,
        };

        document.MapTo<Person>(person, documentMapping);

        person.Name.Should().Be("John Doe");
        person.Age.Should().Be(30);
    }
}