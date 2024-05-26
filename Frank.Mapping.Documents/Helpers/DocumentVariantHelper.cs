using Frank.Mapping.Documents.Models.Enums;

namespace Frank.Mapping.Documents.Helpers;

public static class DocumentVariantHelper
{
    public static DocumentVariant GetDocumentVariant(string document)
    {
        if (document.TrimStart().StartsWith("{"))
        {
            return DocumentVariant.Json;
        }
        if (document.TrimStart().StartsWith("<"))
        {
            return DocumentVariant.Xml;
        }
        throw new ArgumentException("Document variant not recognized.");
    }
}