using System.Xml.XPath;

namespace Frank.Mapping.Documents.Path;

public record XPathDefinition : PathDefinition
{
    public XPathDefinition(string path) : base(path)
    {
        EnsureValidXPath(path);
    }

    private static void EnsureValidXPath(string path)
    {
        try
        {
            _ = XPathExpression.Compile(path);
        }
        catch (Exception ex)
        {
            throw new ArgumentException($"The provided XPath '{path}' is invalid.", nameof(path), ex);
        }
    }
}