namespace Frank.Mapping.Tests.Common.TestingInfrastructure.SourceCode;

public static class MappingSourceCode
{
    public const string CompleteResultCode = """
                     using Frank.Mapping;
                     
                     namespace Frank.Mapping.Tests.TestingInfrastructure;
                     
                     public class TestMapper : IMappingDefinition<TestSourceClass, TestDestinationClass>
                     {
                         public TestDestinationClass Map(TestSourceClass source)
                         {
                             return new TestDestinationClass()
                             {
                                 Id = default(Guid),
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
                     
                     public class TestSourceClass
                     {
                         public string Name { get; set; }
                         public int Age { get; set; }
                         
                         public TestSourceCar Car { get; set; }
                         
                         public TestAddress Address { get; set; }
                     }
                     
                     public class TestDestinationClass
                     {
                         public Guid Id { get; set; }
                         public string Name { get; set; }
                         public int Age { get; set; }
                         
                         public TestDestinationCar Car { get; set; }
                         
                         public TestAddress Address { get; set; }
                         
                         public TestAddress? Address2 { get; set; }
                     }
                     
                     public class TestAddress
                     {
                         public string Street { get; set; }
                         public string City { get; set; }
                         public string State { get; set; }
                         public string Zip { get; set; }
                     }
                     
                     public class TestSourceCar
                     {
                         public string Make { get; set; }
                         public string Model { get; set; }
                         public int Year { get; set; }
                         public TestSourceInsurance Insurance { get; set; }
                     }
                     
                     public class TestDestinationCar
                     {
                         public string Make { get; set; }
                         public string Model { get; set; }
                         public int Year { get; set; }
                         public string? LicensePlate { get; set; }
                         public TestDestinationInsurance Insurance { get; set; }
                     }
                     
                     public class TestDestinationInsurance
                     {
                         public string PolicyNumber { get; set; }
                         public string Company { get; set; }
                         public string? Agent { get; set; }
                     }
                     
                     public class TestSourceInsurance
                     {
                         public string PolicyNumber { get; set; }
                         public string Company { get; set; }
                         public string? Agent { get; set; }
                     }
                     """;
    
    public const string TestResultCode = """
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
                                         """;
    
    public const string TestSourceCode = """
                     using Frank.Mapping;
                     
                     namespace Frank.Mapping.Tests.TestingInfrastructure;
                     
                     public class TestMapper : IMappingDefinition<TestSourceClass, TestDestinationClass>
                     {
                         public TestDestinationClass Map(TestSourceClass source)
                         {
                         }
                     }
                     
                     public class TestSourceClass
                     {
                         public string Name { get; set; }
                         public int Age { get; set; }
                         
                         public TestSourceCar Car { get; set; }
                         
                         public TestAddress Address { get; set; }
                     }
                     
                     public class TestDestinationClass
                     {
                         public Guid Id { get; set; }
                         public string Name { get; set; }
                         public int Age { get; set; }
                         
                         public TestDestinationCar Car { get; set; }
                         
                         public TestAddress Address { get; set; }
                         
                         public TestAddress? Address2 { get; set; }
                     }
                     
                     public class TestAddress
                     {
                         public string Street { get; set; }
                         public string City { get; set; }
                         public string State { get; set; }
                         public string Zip { get; set; }
                     }
                     
                     public class TestSourceCar
                     {
                         public string Make { get; set; }
                         public string Model { get; set; }
                         public int Year { get; set; }
                         public TestSourceInsurance Insurance { get; set; }
                     }
                     
                     public class TestDestinationCar
                     {
                         public string Make { get; set; }
                         public string Model { get; set; }
                         public int Year { get; set; }
                         public string? LicensePlate { get; set; }
                         public TestDestinationInsurance Insurance { get; set; }
                     }
                     
                     public class TestDestinationInsurance
                     {
                         public string PolicyNumber { get; set; }
                         public string Company { get; set; }
                         public string? Agent { get; set; }
                     }
                     
                     public class TestSourceInsurance
                     {
                         public string PolicyNumber { get; set; }
                         public string Company { get; set; }
                         public string? Agent { get; set; }
                     }
                     """;
}