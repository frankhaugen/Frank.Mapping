namespace Frank.Mapping.Documents;

public class DocumentMapping(Type documentType, DocumentVariant documentVariant, IReadOnlyList<PropertyMapping> propertyMappings)
{
    public Type DocumentType { get; } = documentType ?? throw new ArgumentNullException(nameof(documentType));
    public DocumentVariant DocumentVariant { get; } = documentVariant;
    public IReadOnlyList<PropertyMapping> PropertyMappings { get; } = propertyMappings ?? throw new ArgumentNullException(nameof(propertyMappings));
}