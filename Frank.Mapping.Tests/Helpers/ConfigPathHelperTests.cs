using System.Text;
using Frank.Mapping.Documents.Helpers;
using JetBrains.Annotations;
using Xunit.Abstractions;

namespace Frank.Mapping.Tests.Helpers;

[TestSubject(typeof(ConfigPathHelper))]
public class ConfigPathHelperTests
{
    private readonly ITestOutputHelper _output;

    public ConfigPathHelperTests(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public void ShouldGetConfigPath()
    {
        // Arrange
        var configJsonContent = """
        {
            "key": "value"
        }
        """;
        
        // Act
        var result = ConfigPathHelper.GetPaths(configJsonContent).ToArray();
        
        // Assert
        Assert.False(string.IsNullOrEmpty(result.FirstOrDefault()));
        _output.WriteLine($"Test Result:");
        _output.WriteLine(result);
    }

    [Fact]
    public void ShouldThrowException_WhenValueIsNotJson()
    {
        // Arrange
        var configXmlContent = "<xml></xml>";
        
        // Act & Assert
        Assert.Throws<ArgumentException>(() => ConfigPathHelper.GetPaths(configXmlContent));
    }
}