using Frank.Mapping.Documents.Models.Enums;

namespace Frank.Mapping.Documents.Models;

public class DocumentMapping<T>(DocumentVariant documentVariant, IReadOnlyList<PropertyMapping> propertyMappings) : DocumentMapping(typeof(T), documentVariant, propertyMappings);