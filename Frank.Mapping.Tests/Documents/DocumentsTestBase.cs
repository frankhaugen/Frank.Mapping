using Xunit.Abstractions;

namespace Frank.Mapping.Tests.Documents;

public abstract class DocumentsTestBase
{
    public readonly ITestOutputHelper _outputHelper;
    
    public DocumentsTestBase(ITestOutputHelper outputHelper)
    {
        _outputHelper = outputHelper;
    }
    
    public static string _jsonDocument = 
        """
        {
            "Name": "John Doe",
            "age": 30,
            "Address": {
                "City": "New York"
            }
        }
        """;
    
    public static string _xmlDocument = 
        """
        <Person>
            <Name>John Doe</Name>
            <age>30</age>
            <Address>
                <City>New York</City>
            </Address>
        </Person>
        """;
    
    public class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
        
        public Address Address { get; set; }
    }
    
    public class Address
    {
        public string? Street { get; set; }
        public string City { get; set; }
    }
}