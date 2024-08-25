namespace Frank.Mapping.Tests.Common.TestingInfrastructure;

public class TestDestinationClass
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
    
    public TestDestinationCar Car { get; set; }
    
    public TestAddress Address { get; set; }
    
    public TestAddress? Address2 { get; set; }
}