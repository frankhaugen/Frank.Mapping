using Frank.Mapping.Documents.Helpers;
using JetBrains.Annotations;

namespace Frank.Mapping.Tests.Helpers;

[TestSubject(typeof(ObjectHelper))]
public class ObjectHelperTests
{
    [Test]
    public async Task ShouldGetObjectProperties()
    {
        // Arrange
        var obj = new { Name = "John", Age = 30 };
        
        // Act
        var result = ObjectHelper.CreateInstance<Dictionary<string, object>>();
        
        result.Add("Name", obj.Name);
        result.Add("Age", obj.Age);
        
        // Assert
        await Assert.That(result).IsNotNull();
        Console.WriteLine($"Test Result:");
        Console.WriteLine(string.Join(Environment.NewLine, result.Select(kvp => $"{kvp.Key}: {kvp.Value}")));
    }
    
    [Test]
    public async Task ShouldGetObjectPropertiesAsync()
    {
        // Arrange
        var obj = new { Name = "John", Age = 30 };
        
        
        // Act
        var result = ObjectHelper.CreateInstanceFromValues<Dictionary<string, object>>(new Dictionary<string, string?>() { { "Name", obj.Name }, { "Age", obj.Age.ToString() } });
        
        // Assert
        await Assert.That(result).IsNotNull();
        Console.WriteLine($"Test Result:");
        Console.WriteLine(string.Join(Environment.NewLine, result.Select(kvp => $"{kvp.Key}: {kvp.Value}")));
        
        await Assert.That(result["Name"]).IsEqualTo("John");
        await Assert.That(result["Age"]).IsEqualTo("30");
    }
}