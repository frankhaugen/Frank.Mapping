using Frank.Mapping.Documents;
using Frank.Mapping.Documents.Extensions;
using Frank.Mapping.Documents.Path;
using Frank.Mapping.Tests.Common.TestingInfrastructure;
using Frank.Mapping.Tests.Documents;
using JetBrains.Annotations;

namespace Frank.Mapping.Tests.Extensions;

[TestSubject(typeof(JsonPathDefinitionExtensions))]
public class JsonPathDefinitionExtensionsTests : DocumentsTestBase
{
    [Test]
    public async Task GetValue_CreatesCorrectJsonPathDefinition()
    {
        // Arrange
        var jsonPath = "$.Car.Insurance.PolicyNumber";
        
        // Act
        var result = PathDefinition.Create<MyPath>(jsonPath);
        
        // Assert
        Console.WriteLine(result?.ToString());
        await Assert.That(result).IsEqualTo(new MyPath(jsonPath));
        
        var myType = new TestSourceClass()
        {
            Name = "John Doe",
            Age = 30,
            Car = new TestSourceCar()
            {
                Make = "Toyota",
                Model = "Camry",
                Year = 2020,
                Insurance = new TestSourceInsurance()
                {
                    PolicyNumber = "123456"
                }
            },
            Address = new TestAddress()
            {
                Street = "123 Main St",
                City = "Anytown",
                State = "NY",
                Zip = "12345"
            }
        };
        
        var value = result!.GetValue(myType);
        Console.WriteLine(value);
        await Assert.That(value).IsEqualTo("123456");
        
        var genericValue = result.GetValue<string>(myType);
        Console.WriteLine(genericValue);
        await Assert.That(genericValue).IsEqualTo("123456");
        
        await Assert.That(genericValue).IsEqualTo((string?)value);
    }
}

public record MyPath : JsonPathDefinition
{
    /// <inheritdoc />
    public MyPath(string path) : base(path)
    {
    }
}