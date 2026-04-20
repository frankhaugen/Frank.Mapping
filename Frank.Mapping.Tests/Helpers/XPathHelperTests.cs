using Frank.Mapping.Documents.Helpers;
using JetBrains.Annotations;

namespace Frank.Mapping.Tests.Helpers;

[TestSubject(typeof(XPathHelper))]
public class XPathHelperTests
{
    [Test]
    public async Task ShouldGetXPath()
    {
        // Arrange
        var xml = 
            """
            <root>
                <key>value</key>
                <nested>
                    <key1>value1</key1>
                </nested>
                <array>
                    <item>
                        <key2>value2</key2>
                    </item>
                </array>
            </root>
            """;
        
        // Act
        var result = XPathHelper.GetPaths(xml).ToArray();
        
        // Assert
        await Assert.That(result).IsNotNull();
        Console.WriteLine($"Test Result:");
        Console.WriteLine(string.Join(Environment.NewLine, result));
    }
}