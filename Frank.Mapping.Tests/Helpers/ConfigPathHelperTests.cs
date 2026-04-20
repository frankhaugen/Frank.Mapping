using System.Text;
using Frank.Mapping.Documents.Helpers;
using JetBrains.Annotations;

namespace Frank.Mapping.Tests.Helpers;

[TestSubject(typeof(ConfigPathHelper))]
public class ConfigPathHelperTests
{
    [Test]
    public async Task ShouldGetConfigPath()
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
        await Assert.That(result.FirstOrDefault()).IsNotNull();
        Console.WriteLine($"Test Result:");
        Console.WriteLine(string.Join(Environment.NewLine, result));
    }

    [Test]
    public async Task ShouldThrowException_WhenValueIsNotJson()
    {
        // Arrange
        var configXmlContent = "<xml></xml>";
        
        // Act & Assert
        await Assert.That(() => ConfigPathHelper.GetPaths(configXmlContent)).ThrowsExactly<ArgumentException>();
    }
}