using Frank.Mapping.Documents.Helpers;
using Frank.Mapping.Documents.Models;

namespace Frank.Mapping.Documents;

public class DocumentValuesExtractor(List<ValuePath> valuePaths)
{
    public IEnumerable<ValuePathResult> ExtractValuesFromDocument(string document)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(document, nameof(document));
        var documentVariant = DocumentVariantHelper.GetDocumentVariant(document);
        
        var values = new List<ValuePathResult>();
        foreach (var valuePath in valuePaths)
        {
            if (valuePath.DocumentVariant != documentVariant)
                throw new ArgumentException($"Value path {valuePath.Path} is not of the correct variant.");
            var value = valuePath.GetValue(document);
            values.Add(new ValuePathResult(valuePath, value));
        }
        return values;
    }
}