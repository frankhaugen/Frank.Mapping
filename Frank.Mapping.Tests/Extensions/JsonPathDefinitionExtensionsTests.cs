using Frank.Mapping.Documents;
using Frank.Mapping.Documents.Extensions;
using Frank.Mapping.Documents.Path;
using Frank.Mapping.Tests.Common.TestingInfrastructure;
using Frank.Mapping.Tests.Documents;
using JetBrains.Annotations;
using Xunit.Abstractions;

namespace Frank.Mapping.Tests.Extensions;

[TestSubject(typeof(JsonPathDefinitionExtensions))]
public class JsonPathDefinitionExtensionsTests : DocumentsTestBase
{
    /// <inheritdoc />
    public JsonPathDefinitionExtensionsTests(ITestOutputHelper outputHelper) : base(outputHelper)
    {
    }
    
    [Fact]
    public void GetValue_CreatesCorrectJsonPathDefinition()
    {
        // Arrange
        var jsonPath = "$.Car.Insurance.PolicyNumber";
        
        // Act
        var result = PathDefinition.Create<MyPath>(jsonPath);
        
        // Assert
        _outputHelper.WriteLine(result);
        Assert.Equivalent(new MyPath(jsonPath), result);
        
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
        
        var value = result.GetValue(myType);
        _outputHelper.WriteLine(value);
        Assert.Equal("123456", value);
        
        var genericValue = result.GetValue<string>(myType);
        _outputHelper.WriteLine(genericValue);
        Assert.Equal("123456", genericValue);
        
        Assert.Equal(genericValue, value);
    }
}

public record MyPath : JsonPathDefinition
{
    /// <inheritdoc />
    public MyPath(string path) : base(path)
    {
    }
}