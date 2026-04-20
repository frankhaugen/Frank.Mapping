using Frank.Mapping.Documents.Helpers;
using JetBrains.Annotations;

namespace Frank.Mapping.Tests.Helpers;

[TestSubject(typeof(ThrowHelper))]
public class ThrowHelperTests
{
    [Test]
    public async Task ShouldThrowException()
    {
        // Arrange
        var message = "";
        
        // Act & Assert
        await Assert.That(() => ThrowHelper.ThrowIfNullOrWhiteSpace(message, nameof(message))).ThrowsExactly<ArgumentException>();
    }
    
    [Test]
    public async Task ShouldThrowException_WhenMessageIsNull()
    {
        // Arrange
        string? message = null;
        
        // Act & Assert
        await Assert.That(() => ThrowHelper.ThrowIfNullOrWhiteSpace(message, nameof(message))).ThrowsExactly<ArgumentException>();
    }
    
    [Test]
    public async Task ShouldThrowException_WhenMessageIsEmpty()
    {
        // Arrange
        var message = "";
        
        // Act & Assert
        await Assert.That(() => ThrowHelper.ThrowIfNullOrWhiteSpace(message, nameof(message))).ThrowsExactly<ArgumentException>();
    }
    
    [Test]
    public async Task ShouldThrowException_WhenMessageIsWhiteSpace()
    {
        // Arrange
        var message = " ";
        
        // Act & Assert
        await Assert.That(() => ThrowHelper.ThrowIfNullOrWhiteSpace(message, nameof(message))).ThrowsExactly<ArgumentException>();
    }
    
    [Test]
    public async Task ShouldThrowException_WhenThereIsExceptions()
    {
        // Act & Assert
        await Assert.That(() => ThrowHelper.ThrowAggregatedExceptionIfAny(new Exception())).ThrowsExactly<AggregateException>();
    }
}