using Frank.Mapping.Documents.Helpers;
using JetBrains.Annotations;

namespace Frank.Mapping.Tests.Nlp;

[TestSubject(typeof(NgramHelper))]
public class NgramTests
{
    [Test]
    public async Task ShouldGetComparison()
    {
        // Arrange
        var text1 = "This is a test";
        var text2 = "This is a test";
        
        // Act
        var result = NgramHelper.Compare(text1, text2);
        
        // Assert
        await Assert.That(result).IsGreaterThanOrEqualTo(0.99);
        await Assert.That(result).IsLessThanOrEqualTo(1.01);
        Console.WriteLine($"Test Result: {result}");
    }
}