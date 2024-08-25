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
    
    [Fact]
    public void Test3_ExtractValues()
    {
        var document = new Document(_jsonDocument);
        var valuePaths = new List<ValuePath>()
        {
            new ValuePath(DocumentVariant.Json, "$.Name", typeof(string)),
            new ValuePath(DocumentVariant.Json, "$.age", typeof(int)),
        };

        var values = document.ExtractValues(valuePaths).ToList();

        values[0].Value.Should().Be("John Doe");
        values[1].Value.Should().Be(30);
        
        var xmlDocument = new Document(_xmlDocument);
        var xmlValuePaths = new List<ValuePath>()
        {
            new ValuePath(DocumentVariant.Xml, "/Person/Name", typeof(string)),
            new ValuePath(DocumentVariant.Xml, "/Person/age", typeof(int)),
        };
        
        var xmlValues = xmlDocument.ExtractValues(xmlValuePaths).ToList();
        
        xmlValues[0].Value.Should().Be("John Doe");
        xmlValues[1].Value.Should().Be(30);
        
        values[0].Value.Should().NotBe("/Person/Name");
        values[1].Value.Should().NotBe("/Person/age");
        
        xmlValues[0].Value.Should().NotBe("$.Name");
        xmlValues[1].Value.Should().NotBe("$.age");
        
        Assert.Throws<ArgumentException>(() => document.ExtractValues(new List<ValuePath>()
        {
            new ValuePath(DocumentVariant.Xml, "/Person/Name", typeof(string)),
            new ValuePath(DocumentVariant.Xml, "/Person/age", typeof(int)),
        }));
        
        Assert.Throws<ArgumentException>(() => document.ExtractValues(new List<ValuePath>()
        {
            new ValuePath(DocumentVariant.Json, "$.Name", typeof(string)),
            new ValuePath(DocumentVariant.Xml, "/Person/age", typeof(int)),
        }));
    }
    
    [Fact]
    public void Test4_GetPaths()
    {
        var jsonDocument = new Document(_jsonDocument);
        var jsonPaths = jsonDocument.GetPaths().ToList();

        jsonPaths.Should().Contain("$.Name");
        jsonPaths.Should().Contain("$.age");
        jsonPaths.Should().Contain("$.Address.City");
        
        var xmlDocument = new Document(_xmlDocument);
        var xmlPaths = xmlDocument.GetPaths().ToList();
        
        xmlPaths.Should().Contain("/Person/Name");
        xmlPaths.Should().Contain("/Person/age");
        xmlPaths.Should().Contain("/Person/Address/City");
        
        jsonPaths.Should().NotContain("/Person/Name");
        jsonPaths.Should().NotContain("/Person/age");
        jsonPaths.Should().NotContain("/Person/Address/City");
        
        xmlPaths.Should().NotContain("$.Name");
        xmlPaths.Should().NotContain("$.age");
        xmlPaths.Should().NotContain("$.Address.City");
        
        Assert.Throws<ArgumentException>(() => new Document("Invalid document"));
    }
    
    [Fact]
    public void Test5_MapTo()
    {
        var document = new Document(_jsonDocument);
        var person = new Person();
        var documentMapping = new DocumentMapping<Person>(DocumentVariant.Json, new List<PropertyMapping>()
        {
            new PropertyMapping<Person, string>(d => d.Name, new ValuePath(DocumentVariant.Json, "$.Name", typeof(string))),
            new PropertyMapping<Person, int>(d => d.Age, new ValuePath(DocumentVariant.Json, "$.age", typeof(int))),
        });

        document.MapTo(person, documentMapping);

        person.Name.Should().Be("John Doe");
        person.Age.Should().Be(30);
        
        var xmlDocument = new Document(_xmlDocument);
        var xmlPerson = new Person();
        var xmlDocumentMapping = new DocumentMapping<Person>(DocumentVariant.Xml, new List<PropertyMapping>()
        {
            new PropertyMapping<Person, string>(d => d.Name, new ValuePath(DocumentVariant.Xml, "/Person/Name", typeof(string))),
            new PropertyMapping<Person, int>(d => d.Age, new ValuePath(DocumentVariant.Xml, "/Person/age", typeof(int))),
        });
        
        xmlDocument.MapTo(xmlPerson, xmlDocumentMapping);
        
        xmlPerson.Name.Should().Be("John Doe");
        xmlPerson.Age.Should().Be(30);
    }
    
    [Fact]
    public void Test6_MapTo()
    {
        var document = new Document(_jsonDocument);
        var documentMapping = new DocumentMapping<Person>(DocumentVariant.Json, new List<PropertyMapping>()
        {
            new PropertyMapping<Person, string>(d => d.Name, new ValuePath(DocumentVariant.Json, "$.Name", typeof(string))),
            new PropertyMapping<Person, int>(d => d.Age, new ValuePath(DocumentVariant.Json, "$.age", typeof(int))),
        });

        var person = document.MapTo<Person>(documentMapping);

        person.Name.Should().Be("John Doe");
        person.Age.Should().Be(30);
    }
    
    [Fact]
    public void Test7_MapTo()
    {
        var document = new Document(_jsonDocument);
        var documentMapping = new DocumentMapping<Person>(DocumentVariant.Json, new List<PropertyMapping>()
        {
            new PropertyMapping<Person, string>(d => d.Name, new ValuePath(DocumentVariant.Json, "$.Name", typeof(string))),
            new PropertyMapping<Person, int>(d => d.Age, new ValuePath(DocumentVariant.Json, "$.age", typeof(int))),
        });

        var person = document.MapTo<Person>(documentMapping);

        person.Name.Should().Be("John Doe");
        person.Age.Should().Be(30);
    }
}