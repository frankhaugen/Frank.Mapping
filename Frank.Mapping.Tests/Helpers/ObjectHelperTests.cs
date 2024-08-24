using FluentAssertions;
using Frank.Mapping.Documents.Helpers;
using JetBrains.Annotations;
using Xunit.Abstractions;

namespace Frank.Mapping.Tests.Helpers;

[TestSubject(typeof(ObjectHelper))]
public class ObjectHelperTests
{
    private readonly ITestOutputHelper _outputHelper;

    public ObjectHelperTests(ITestOutputHelper outputHelper)
    {
        _outputHelper = outputHelper;
    }
    
    [Fact]
    public void ShouldGetObjectProperties()
    {
        // Arrange
        var obj = new { Name = "John", Age = 30 };
        
        // Act
        var result = ObjectHelper.CreateInstance<Dictionary<string, object>>();
        
        result.Add("Name", obj.Name);
        result.Add("Age", obj.Age);
        
        // Assert
        result.Should().NotBeNull();
        _outputHelper.WriteLine($"Test Result:");
        _outputHelper.WriteLine(result);
    }
    
    [Fact]
    public void ShouldGetObjectPropertiesAsync()
    {
        // Arrange
        var obj = new { Name = "John", Age = 30 };
        
        
        // Act
        var result = ObjectHelper.CreateInstanceFromValues<Dictionary<string, object>>(new Dictionary<string, string?>() { { "Name", obj.Name }, { "Age", obj.Age.ToString() } });
        
        // Assert
        result.Should().NotBeNull();
        _outputHelper.WriteLine($"Test Result:");
        _outputHelper.WriteLine(result);
        
        result["Name"].Should().Be("John");
        result["Age"].Should().Be("30");
    }
}