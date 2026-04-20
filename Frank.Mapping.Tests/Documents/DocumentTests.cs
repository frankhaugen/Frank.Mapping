using Frank.Mapping.Documents;
using Frank.Mapping.Documents.Models;
using Frank.Mapping.Documents.Models.Enums;

namespace Frank.Mapping.Tests.Documents;

public class DocumentTests : DocumentsTestBase
{
    [Test]
    public async Task Test()
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

        await Assert.That(person.Name).IsEqualTo("John Doe");
        await Assert.That(person.Age).IsEqualTo(30);
    }
    
    [Test]
    public async Task Test2()
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

        await Assert.That(person.Name).IsEqualTo("John Doe");
        await Assert.That(person.Age).IsEqualTo(30);
    }
    
    [Test]
    public async Task Test3_ExtractValues()
    {
        var document = new Document(_jsonDocument);
        var valuePaths = new List<ValuePath>()
        {
            new ValuePath(DocumentVariant.Json, "$.Name", typeof(string)),
            new ValuePath(DocumentVariant.Json, "$.age", typeof(int)),
        };

        var values = document.ExtractValues(valuePaths).ToList();

        await Assert.That(values[0].Value).IsEqualTo("John Doe");
        await Assert.That(Convert.ToInt32(values[1].Value)).IsEqualTo(30);
        
        var xmlDocument = new Document(_xmlDocument);
        var xmlValuePaths = new List<ValuePath>()
        {
            new ValuePath(DocumentVariant.Xml, "/Person/Name", typeof(string)),
            new ValuePath(DocumentVariant.Xml, "/Person/age", typeof(int)),
        };
        
        var xmlValues = xmlDocument.ExtractValues(xmlValuePaths).ToList();
        
        await Assert.That(xmlValues[0].Value).IsEqualTo("John Doe");
        await Assert.That(Convert.ToInt32(xmlValues[1].Value)).IsEqualTo(30);
        
        await Assert.That(values[0].Value).IsNotEqualTo("/Person/Name");
        await Assert.That(values[1].Value).IsNotEqualTo("/Person/age");
        
        await Assert.That(xmlValues[0].Value).IsNotEqualTo("$.Name");
        await Assert.That(xmlValues[1].Value).IsNotEqualTo("$.age");
        
        await Assert.That(() => document.ExtractValues(new List<ValuePath>()
        {
            new ValuePath(DocumentVariant.Xml, "/Person/Name", typeof(string)),
            new ValuePath(DocumentVariant.Xml, "/Person/age", typeof(int)),
        })).ThrowsExactly<ArgumentException>();
        
        await Assert.That(() => document.ExtractValues(new List<ValuePath>()
        {
            new ValuePath(DocumentVariant.Json, "$.Name", typeof(string)),
            new ValuePath(DocumentVariant.Xml, "/Person/age", typeof(int)),
        })).ThrowsExactly<ArgumentException>();
    }
    
    [Test]
    public async Task Test4_GetPaths()
    {
        var jsonDocument = new Document(_jsonDocument);
        var jsonPaths = jsonDocument.GetPaths().ToList();

        await Assert.That(jsonPaths).Contains("$.Name");
        await Assert.That(jsonPaths).Contains("$.age");
        await Assert.That(jsonPaths).Contains("$.Address.City");
        
        var xmlDocument = new Document(_xmlDocument);
        var xmlPaths = xmlDocument.GetPaths().ToList();
        
        await Assert.That(xmlPaths).Contains("/Person/Name");
        await Assert.That(xmlPaths).Contains("/Person/age");
        await Assert.That(xmlPaths).Contains("/Person/Address/City");
        
        await Assert.That(jsonPaths).DoesNotContain("/Person/Name");
        await Assert.That(jsonPaths).DoesNotContain("/Person/age");
        await Assert.That(jsonPaths).DoesNotContain("/Person/Address/City");
        
        await Assert.That(xmlPaths).DoesNotContain("$.Name");
        await Assert.That(xmlPaths).DoesNotContain("$.age");
        await Assert.That(xmlPaths).DoesNotContain("$.Address.City");
        
        await Assert.That(() => new Document("Invalid document")).ThrowsExactly<ArgumentException>();
    }
    
    [Test]
    public async Task Test5_MapTo()
    {
        var document = new Document(_jsonDocument);
        var person = new Person();
        var documentMapping = new DocumentMapping<Person>(DocumentVariant.Json, new List<PropertyMapping>()
        {
            new PropertyMapping<Person, string>(d => d.Name, new ValuePath(DocumentVariant.Json, "$.Name", typeof(string))),
            new PropertyMapping<Person, int>(d => d.Age, new ValuePath(DocumentVariant.Json, "$.age", typeof(int))),
        });

        document.MapTo(person, documentMapping);

        await Assert.That(person.Name).IsEqualTo("John Doe");
        await Assert.That(person.Age).IsEqualTo(30);
        
        var xmlDocument = new Document(_xmlDocument);
        var xmlPerson = new Person();
        var xmlDocumentMapping = new DocumentMapping<Person>(DocumentVariant.Xml, new List<PropertyMapping>()
        {
            new PropertyMapping<Person, string>(d => d.Name, new ValuePath(DocumentVariant.Xml, "/Person/Name", typeof(string))),
            new PropertyMapping<Person, int>(d => d.Age, new ValuePath(DocumentVariant.Xml, "/Person/age", typeof(int))),
        });
        
        xmlDocument.MapTo(xmlPerson, xmlDocumentMapping);
        
        await Assert.That(xmlPerson.Name).IsEqualTo("John Doe");
        await Assert.That(xmlPerson.Age).IsEqualTo(30);
    }
    
    [Test]
    public async Task Test6_MapTo()
    {
        var document = new Document(_jsonDocument);
        var documentMapping = new DocumentMapping<Person>(DocumentVariant.Json, new List<PropertyMapping>()
        {
            new PropertyMapping<Person, string>(d => d.Name, new ValuePath(DocumentVariant.Json, "$.Name", typeof(string))),
            new PropertyMapping<Person, int>(d => d.Age, new ValuePath(DocumentVariant.Json, "$.age", typeof(int))),
        });

        var person = document.MapTo<Person>(documentMapping);

        await Assert.That(person.Name).IsEqualTo("John Doe");
        await Assert.That(person.Age).IsEqualTo(30);
    }
    
    [Test]
    public async Task Test7_MapTo()
    {
        var document = new Document(_jsonDocument);
        var documentMapping = new DocumentMapping<Person>(DocumentVariant.Json, new List<PropertyMapping>()
        {
            new PropertyMapping<Person, string>(d => d.Name, new ValuePath(DocumentVariant.Json, "$.Name", typeof(string))),
            new PropertyMapping<Person, int>(d => d.Age, new ValuePath(DocumentVariant.Json, "$.age", typeof(int))),
        });

        var person = document.MapTo<Person>(documentMapping);

        await Assert.That(person.Name).IsEqualTo("John Doe");
        await Assert.That(person.Age).IsEqualTo(30);
    }
}