using System.Xml.XPath;

namespace Frank.Mapping.Documents;

public static class ConfigPathConverter
{
    public static string ConvertFromJsonPath(string jsonPath)
    {
        if (string.IsNullOrWhiteSpace(jsonPath))
            throw new ArgumentException("JsonPath cannot be null or empty.", nameof(jsonPath));

        var segments = new List<string>();
        var jsonPathSegments = jsonPath.Split('.');
        foreach (var jsonPathSegment in jsonPathSegments)
        {
            var segment = jsonPathSegment;
            if (segment.Contains("["))
            {
                var index = segment.IndexOf("[", StringComparison.Ordinal);
                segment = segment.Substring(0, index);
            }
            segments.Add(segment);
        }

        return string.Join(":", segments);
    }
    
    public static string ConvertFromXPath(string xpath)
    {
        if (string.IsNullOrWhiteSpace(xpath))
            throw new ArgumentException("XPath cannot be null or empty.", nameof(xpath));

        var segments = new List<string>();
        var xmlDocument = new XPathDocument(new System.IO.StringReader("<root />"));
        var navigator = xmlDocument.CreateNavigator();

        var xpathExpression = XPathExpression.Compile(xpath);
        var iterator = navigator.Select(xpathExpression);

        while (iterator.MoveNext())
        {
            var current = iterator.Current;
            if (current != null)
            {
                segments.Add(ParseSegment(current));
            }
        }

        return string.Join(":", segments);
    }

    private static string ParseSegment(XPathNavigator navigator)
    {
        var segment = navigator.LocalName;
        if (navigator.HasAttributes)
        {
            var attributes = new List<string>();
            var attributeNavigator = navigator.Clone();
            if (attributeNavigator.MoveToFirstAttribute())
            {
                do
                {
                    attributes.Add($"{attributeNavigator.Name}='{attributeNavigator.Value}'");
                } while (attributeNavigator.MoveToNextAttribute());

                segment += $"[{string.Join(",", attributes)}]";
            }
        }
        return segment;
    }
}