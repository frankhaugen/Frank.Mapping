namespace Frank.Mapping.Tests.Common.TestingInfrastructure;

public class TestSourceCar
{
    public string Make { get; set; }
    public string Model { get; set; }
    public int Year { get; set; }
    public TestSourceInsurance Insurance { get; set; }
}