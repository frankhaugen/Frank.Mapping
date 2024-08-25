namespace Frank.Mapping.Tests.Common.TestingInfrastructure;

public class TestDestinationCar
{
    public string Make { get; set; }
    public string Model { get; set; }
    public int Year { get; set; }
    public string? LicensePlate { get; set; }
    public TestDestinationInsurance Insurance { get; set; }
}