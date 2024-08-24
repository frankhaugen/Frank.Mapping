using System.Reflection;
using Frank.Mapping.Documents.Path;

namespace Frank.Mapping.Documents.Models;

public record PropertyReference
{
    public required Type Type { get; init; }
    
    public required PropertyInfo PropertyInfo { get; init; }
    
    public required JsonPathDefinition JsonPath { get; init; }
    
    public required XPathDefinition XPath { get; init; }
    
    public required ConfigPathDefinition ConfigPathDefinition { get; init; }
}