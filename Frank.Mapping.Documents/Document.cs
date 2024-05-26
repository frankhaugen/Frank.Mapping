using Frank.Mapping.Documents.Helpers;
using Frank.Mapping.Documents.Models;
using Frank.Mapping.Documents.Models.Enums;

namespace Frank.Mapping.Documents;

public class Document(string value)
{
    public string Value { get; } = value;

    public DocumentVariant DocumentVariant { get; } = DocumentVariantHelper.GetDocumentVariant(value);

    public IEnumerable<ValuePathResult> ExtractValues(List<ValuePath> valuePaths)
    {
        var values = new List<ValuePathResult>();
        foreach (var valuePath in valuePaths)
        {
            if (valuePath.DocumentVariant != DocumentVariant)
                throw new ArgumentException($"Value path {valuePath.Path} is not of the correct variant.");
            var value = valuePath.GetValue(Value);
            values.Add(new ValuePathResult(valuePath, value));
        }

        return values;
    }

    public void MapTo<T>(T instance, IEnumerable<PropertyMapping> propertyMappings)
    {
        ArgumentNullException.ThrowIfNull(instance, nameof(instance));
        foreach (var propertyMapping in propertyMappings) propertyMapping.Map(Value, instance);
    }

    public void MapTo<T>(T instance, DocumentMapping<T> documentMapping)
    {
        ArgumentNullException.ThrowIfNull(instance, nameof(instance));
        ArgumentNullException.ThrowIfNull(documentMapping, nameof(documentMapping));

        if (documentMapping.DocumentVariant != DocumentVariant)
            throw new ArgumentException($"Document variant {DocumentVariant} does not match document mapping variant {documentMapping.DocumentVariant}");

        MapTo(instance, documentMapping.PropertyMappings);
    }

    public T MapTo<T>(DocumentMapping<T> documentMapping)
    {
        ArgumentNullException.ThrowIfNull(documentMapping, nameof(documentMapping));
        var instance = Activator.CreateInstance<T>();
        MapTo(instance, documentMapping.PropertyMappings);
        return instance;
    }

    public IEnumerable<string> GetPaths()
    {
        return DocumentVariant switch
        {
            DocumentVariant.Json => JsonPathHelper.GetPaths(Value),
            DocumentVariant.Xml => XPathHelper.GetPaths(Value),
            _ => throw new NotImplementedException($"Document variant {DocumentVariant} is not implemented.")
        };
    }
}