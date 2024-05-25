namespace Frank.Mapping.Documents;

public class DocumentMapping<T>(DocumentVariant documentVariant, IReadOnlyList<PropertyMapping> propertyMappings) : DocumentMapping(typeof(T), documentVariant, propertyMappings);