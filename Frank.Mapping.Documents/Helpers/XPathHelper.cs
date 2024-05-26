using System.Xml;
using System.Xml.Linq;
using Frank.Mapping.Documents.Models.Enums;

namespace Frank.Mapping.Documents.Helpers;

public static class XPathHelper
{
    public static IEnumerable<string> GetPaths(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value, nameof(value));

        var documentVariant = DocumentVariantHelper.GetDocumentVariant(value);

        if (documentVariant != DocumentVariant.Xml)
            throw new ArgumentException("Value is not an XML document.");

        var xml = RemoveAllNamespaces(value);
        return ExtractAllXPaths(xml);
    }

    private static IEnumerable<string> ExtractAllXPaths(string xml)
    {
        var xpaths = new List<string>();
        using var reader = XmlReader.Create(new StringReader(xml));
        var elementStack = new Stack<string>();
        while (reader.Read())
            switch (reader.NodeType)
            {
                case XmlNodeType.Element:
                    var elementPath = "/" + reader.Name;
                    if (elementStack.Count > 0)
                        elementPath = elementStack.Peek() + elementPath;

                    elementStack.Push(elementPath);
                    xpaths.Add(elementPath);

                    if (reader.HasAttributes)
                    {
                        while (reader.MoveToNextAttribute())
                            xpaths.Add($"{elementPath}/@{reader.Name}");

                        reader.MoveToElement();
                    }

                    break;

                case XmlNodeType.EndElement:
                    elementStack.Pop();
                    break;
            }

        return xpaths;
    }

    private static string RemoveAllNamespaces(string xmlDocument) => RemoveAllNamespaces(XElement.Parse(xmlDocument)).ToString();

    private static XElement RemoveAllNamespaces(XElement e) =>
        new(e.Name.LocalName,
            from n in e.Nodes()
            select n is XElement ? RemoveAllNamespaces(n as XElement) : n,
            e.HasAttributes
                ? from a in e.Attributes()
                where !a.IsNamespaceDeclaration
                select new XAttribute(a.Name.LocalName, a.Value)
                : null);
}