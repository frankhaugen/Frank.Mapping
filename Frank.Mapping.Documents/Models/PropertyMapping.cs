using System.Reflection;
using Frank.Mapping.Documents.Extensions;

namespace Frank.Mapping.Documents.Models;

public class PropertyMapping
{
    public PropertyInfo Property { get; }
    public ValuePath ValuePath { get; }
    public Type Type { get; }

    public PropertyMapping(PropertyInfo property, ValuePath valuePath, Type type)
    {
        ArgumentNullException.ThrowIfNull(property, nameof(property));
        ArgumentNullException.ThrowIfNull(valuePath, nameof(valuePath));
        ArgumentNullException.ThrowIfNull(type, nameof(type));
        Property = property;
        ValuePath = valuePath;
        Type = type;
    }
    
    public object? GetValue(string document)
    {
        return ValuePath.GetValue(document);
    }
    
    public void Map(string document, object instance)
    {
        if (instance.GetType() != Type)
            throw new Exception($"Instance {instance.GetType().GetFullName()} does not match expected type: {Type.GetFullName()}");
        
        var value = GetValue(document);
        
        if (value == null)
            throw new Exception($"Failed to get value from document using path: {ValuePath.Path}");
        
        var convertedValue = Convert.ChangeType(value, Property.PropertyType);
        
        Property.SetValue(instance, convertedValue);
    }
}