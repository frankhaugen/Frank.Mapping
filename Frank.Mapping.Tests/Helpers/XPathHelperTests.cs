using Frank.Mapping.Documents.Helpers;
using JetBrains.Annotations;
using Xunit.Abstractions;

namespace Frank.Mapping.Tests.Helpers;

[TestSubject(typeof(XPathHelper))]
public class XPathHelperTests
{
    private readonly ITestOutputHelper _outputHelper;
    
    public XPathHelperTests(ITestOutputHelper outputHelper)
    {
        _outputHelper = outputHelper;
    }
    
    [Fact]
    public void ShouldGetXPath()
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
        Assert.NotNull(result);
        _outputHelper.WriteLine($"Test Result:");
        _outputHelper.WriteLine(result);
    }
}