using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Frank.Mapping.Documents.Path;

namespace Frank.Mapping.Documents.Extensions;

public static partial class ExpressionExtensions
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
    
    public static JsonPathDefinition GetJsonPathDefinition<T, TProp>(this Expression<Func<T, TProp>> propertyExpression)
    {
        var expressionString = propertyExpression.ToString();
        var jsonPathString = ConvertToJsonPath(expressionString);
        return new JsonPathDefinition(jsonPathString);
    }
    
    public static XPathDefinition GetXPathDefinition<T, TProp>(this Expression<Func<T, TProp>> propertyExpression)
    {
        var expressionString = propertyExpression.ToString();
        var xPathString = ConvertToXPath(expressionString);
        return new XPathDefinition(xPathString);
    }
    
    public static ConfigPathDefinition GetConfigPathDefinition<T, TProp>(this Expression<Func<T, TProp>> propertyExpression)
    {
        var configPathString = ConvertToConfigPath(propertyExpression);
        return new ConfigPathDefinition(configPathString);
    }

    private static string ConvertToConfigPath<T, TProp>(Expression<Func<T, TProp>> propertyExpression)
    {
        // var expressionString = propertyExpression.ToString();
        // var propertyPath = expressionString.Substring(expressionString.IndexOf('.') + 1);
        // return propertyPath;
        
        var propertyExpressionWithoutParameter = propertyExpression.Body.ToString().Split('.', 2).Skip(1).Single();
        var key = propertyExpressionWithoutParameter.Replace('.', ':').Replace('[', ':').Replace("]", "");
        key = IndexerAccessRegex().Replace(key, "$1");
        return key;
    }
    
    [GeneratedRegex(@"get_Item\((\d+)\)", RegexOptions.Compiled)]
    private static partial Regex IndexerAccessRegex();

    private static string ConvertToXPath(string expressionString)
    {
        var pathBuilder = new StringBuilder();

        // Remove the leading "x => x." part
        var propertyPath = expressionString.Substring(expressionString.IndexOf('.') + 1);

        foreach (var part in propertyPath.Split('.'))
        {
            if (part.Contains("["))
            {
                // Handle indexers
                var parts = part.Split(new[] { '[', ']' }, StringSplitOptions.RemoveEmptyEntries);
                pathBuilder.Append($"/{parts[0]}[{parts[1]}]");
            }
            else
            {
                pathBuilder.Append($"/{part}");
            }
        }

        return pathBuilder.ToString();
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