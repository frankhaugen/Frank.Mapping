using System.Diagnostics.CodeAnalysis;
using Frank.Mapping.Tests.Common.TestingInfrastructure;

namespace Frank.Mapping.Tests.ConsoleApp;

[ExcludeFromCodeCoverage]
public class TestMapper : IMappingDefinition<TestSourceClass, TestDestinationClass>
{
    public TestDestinationClass Map(TestSourceClass source)
    {
        return new TestDestinationClass()
        {
            Id = default,
            Name = source.Name,
            Age = source.Age,
            Car = new TestDestinationCar()
            {
                Make = source.Car.Make,
                Model = source.Car.Model,
                Year = source.Car.Year,
                LicensePlate = null,
                Insurance = new TestDestinationInsurance()
                {
                    PolicyNumber = source.Car.Insurance.PolicyNumber,
                    Company = source.Car.Insurance.Company,
                    Agent = source.Car.Insurance.Agent
                }
            },
            Address = source.Address,
            Address2 = null
        };
    }
}