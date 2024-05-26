using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Json.Path;

namespace Frank.Mapping.Documents.Models;

public static class PropertyReferenceFactory
{
    // public static PropertyReference Create<T, TProp>(
    // {
    //     return new PropertyReference
    //     {
    //         Type = type,
    //         PropertyInfo = propertyInfo,
    //         JsonPath = jsonPath,
    //         XPath = xPath,
    //         ConfigPathDefinition = configPathDefinition
    //     };
    // }
}

public record PropertyReference
{
    public required Type Type { get; init; }
    
    public required PropertyInfo PropertyInfo { get; init; }
    
    public required JsonPathDefinition JsonPath { get; init; }
    
    public required XPathDefinition XPath { get; init; }
    
    public required ConfigPathDefinition ConfigPathDefinition { get; init; }
}

public record JsonPathDefinition : PathDefinition
{
    public JsonPathDefinition(string path) : base(path)
    {
    }
}

public record XPathDefinition : PathDefinition
{
    public XPathDefinition(string path) : base(path)
    {
    }
}

public record ConfigPathDefinition : PathDefinition
{
    public ConfigPathDefinition(string path) : base(path)
    {
    }
}

public abstract record PathDefinition
{
    public string Path { get; }

    protected PathDefinition(string path)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(path, nameof(path));
        Path = path;
    }
}

public static class ExpressionExtensions
{
    public static PropertyInfo GetPropertyInfo<T, TProp>(this Expression<Func<T, TProp>> propertyExpression)
    {
        ArgumentNullException.ThrowIfNull(propertyExpression, nameof(propertyExpression));
        var memberExpression = propertyExpression.Body as MemberExpression;
        if (memberExpression != null)
            return memberExpression?.Member as PropertyInfo
                   ?? throw new ArgumentException("Expression is not a valid property expression.");
        if (propertyExpression.Body is UnaryExpression { Operand: MemberExpression operand }) memberExpression = operand;

        return memberExpression?.Member as PropertyInfo
               ?? throw new ArgumentException("Expression is not a valid property expression.");
    }
    
    public static JsonPathDefinition GetPathDefinition<T, TProp>(this Expression<Func<T, TProp>> propertyExpression)
    {
        var jsonPath = propertyExpression.GetJsonPath();
        return new JsonPathDefinition(jsonPath.ToString());
    }    
    
    public static JsonPath GetJsonPath<T, TProp>(this Expression<Func<T, TProp>> propertyExpression)
    {
        var expressionString = propertyExpression.ToString();
        var jsonPath = ConvertToJsonPath(expressionString);
        return JsonPath.Parse(jsonPath);
    }

    private static string ConvertToJsonPath(string expressionString)
    {
        var pathBuilder = new StringBuilder("$");

        // Remove the leading "x => x." part
        var propertyPath = expressionString.Substring(expressionString.IndexOf('.') + 1);

        foreach (var part in propertyPath.Split('.'))
        {
            if (part.Contains("["))
            {
                // Handle indexers
                var parts = part.Split(new[] { '[', ']' }, StringSplitOptions.RemoveEmptyEntries);
                pathBuilder.Append($".{parts[0]}[{parts[1]}]");
            }
            else
            {
                pathBuilder.Append($".{part}");
            }
        }

        return pathBuilder.ToString();
    }

}
