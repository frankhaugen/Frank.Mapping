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

        Assert.Equal("John Doe", person.Name);
        Assert.Equal(30, person.Age);
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

        Assert.Equal("John Doe", person.Name);
        Assert.Equal(30, person.Age);
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

        Assert.Equal("John Doe", values[0].Value);
        Assert.Equal(30, Convert.ToInt32(values[1].Value));
        
        var xmlDocument = new Document(_xmlDocument);
        var xmlValuePaths = new List<ValuePath>()
        {
            new ValuePath(DocumentVariant.Xml, "/Person/Name", typeof(string)),
            new ValuePath(DocumentVariant.Xml, "/Person/age", typeof(int)),
        };
        
        var xmlValues = xmlDocument.ExtractValues(xmlValuePaths).ToList();
        
        Assert.Equal("John Doe", xmlValues[0].Value);
        Assert.Equal(30, Convert.ToInt32(xmlValues[1].Value));
        
        Assert.NotEqual("/Person/Name", values[0].Value);
        Assert.NotEqual("/Person/age", values[1].Value);
        
        Assert.NotEqual("$.Name", xmlValues[0].Value);
        Assert.NotEqual("$.age", xmlValues[1].Value);
        
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

        Assert.Contains("$.Name", jsonPaths);
        Assert.Contains("$.age", jsonPaths);
        Assert.Contains("$.Address.City", jsonPaths);
        
        var xmlDocument = new Document(_xmlDocument);
        var xmlPaths = xmlDocument.GetPaths().ToList();
        
        Assert.Contains("/Person/Name", xmlPaths);
        Assert.Contains("/Person/age", xmlPaths);
        Assert.Contains("/Person/Address/City", xmlPaths);
        
        Assert.DoesNotContain("/Person/Name", jsonPaths);
        Assert.DoesNotContain("/Person/age", jsonPaths);
        Assert.DoesNotContain("/Person/Address/City", jsonPaths);
        
        Assert.DoesNotContain("$.Name", xmlPaths);
        Assert.DoesNotContain("$.age", xmlPaths);
        Assert.DoesNotContain("$.Address.City", xmlPaths);
        
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

        Assert.Equal("John Doe", person.Name);
        Assert.Equal(30, person.Age);
        
        var xmlDocument = new Document(_xmlDocument);
        var xmlPerson = new Person();
        var xmlDocumentMapping = new DocumentMapping<Person>(DocumentVariant.Xml, new List<PropertyMapping>()
        {
            new PropertyMapping<Person, string>(d => d.Name, new ValuePath(DocumentVariant.Xml, "/Person/Name", typeof(string))),
            new PropertyMapping<Person, int>(d => d.Age, new ValuePath(DocumentVariant.Xml, "/Person/age", typeof(int))),
        });
        
        xmlDocument.MapTo(xmlPerson, xmlDocumentMapping);
        
        Assert.Equal("John Doe", xmlPerson.Name);
        Assert.Equal(30, xmlPerson.Age);
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

        Assert.Equal("John Doe", person.Name);
        Assert.Equal(30, person.Age);
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

        Assert.Equal("John Doe", person.Name);
        Assert.Equal(30, person.Age);
    }
}