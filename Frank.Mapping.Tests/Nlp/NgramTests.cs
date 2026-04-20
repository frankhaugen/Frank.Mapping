using Frank.Mapping.Documents.Helpers;
using JetBrains.Annotations;
using Xunit.Abstractions;

namespace Frank.Mapping.Tests.Nlp;

[TestSubject(typeof(NgramHelper))]
public class NgramTests
{
    private readonly ITestOutputHelper _outputHelper;

    public NgramTests(ITestOutputHelper outputHelper)
    {
        _outputHelper = outputHelper;
    }
    
    [Fact]
    public void ShouldGetComparison()
    {
        // Arrange
        var text1 = "This is a test";
        var text2 = "This is a test";
        
        // Act
        var result = NgramHelper.Compare(text1, text2);
        
        // Assert
        Assert.InRange(result, 0.99, 1.01);
        _outputHelper.WriteLine($"Test Result: {result}");
    }
}