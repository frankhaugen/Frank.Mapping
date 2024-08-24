using Frank.Mapping.Documents.Helpers;
using JetBrains.Annotations;
using Xunit.Abstractions;

namespace Frank.Mapping.Tests.Helpers;

[TestSubject(typeof(ThrowHelper))]
public class ThrowHelperTests
{
    private readonly ITestOutputHelper _outputHelper;

    public ThrowHelperTests(ITestOutputHelper outputHelper)
    {
        _outputHelper = outputHelper;
    }
    
    [Fact]
    public void ShouldThrowException()
    {
        // Arrange
        var message = "";
        
        // Act & Assert
        Assert.Throws<ArgumentException>(() => ThrowHelper.ThrowIfNullOrWhiteSpace(message, nameof(message)));
    }
    
    [Fact]
    public void ShouldThrowException_WhenMessageIsNull()
    {
        // Arrange
        string? message = null;
        
        // Act & Assert
        Assert.Throws<ArgumentException>(() => ThrowHelper.ThrowIfNullOrWhiteSpace(message, nameof(message)));
    }
    
    [Fact]
    public void ShouldThrowException_WhenMessageIsEmpty()
    {
        // Arrange
        var message = "";
        
        // Act & Assert
        Assert.Throws<ArgumentException>(() => ThrowHelper.ThrowIfNullOrWhiteSpace(message, nameof(message)));
    }
    
    [Fact]
    public void ShouldThrowException_WhenMessageIsWhiteSpace()
    {
        // Arrange
        var message = " ";
        
        // Act & Assert
        Assert.Throws<ArgumentException>(() => ThrowHelper.ThrowIfNullOrWhiteSpace(message, nameof(message)));
    }
    
    [Fact]
    public void ShouldThrowException_WhenThereIsExceptions()
    {
        // Act & Assert
        Assert.Throws<AggregateException>(() => ThrowHelper.ThrowAggregatedExceptionIfAny(new Exception()));
    }
}