using System.Diagnostics.CodeAnalysis;

namespace Frank.Mapping.Tests.Documents;

public abstract class DocumentsTestBase
{
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
    
    [ExcludeFromCodeCoverage]
    public class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
        
        public Address Address { get; set; }
    }
    
    [ExcludeFromCodeCoverage]
    public class Address
    {
        public string? Street { get; set; }
        public string City { get; set; }
    }
}