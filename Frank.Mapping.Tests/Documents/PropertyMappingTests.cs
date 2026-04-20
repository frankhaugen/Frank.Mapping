using Frank.Mapping.Documents;
using Frank.Mapping.Documents.Models;
using Frank.Mapping.Documents.Models.Enums;
using Xunit.Abstractions;

namespace Frank.Mapping.Tests.Documents;

public class PropertyMappingTests : DocumentsTestBase
{
    /// <inheritdoc />
    public PropertyMappingTests(ITestOutputHelper outputHelper) : base(outputHelper)
    {
    }

    [Fact]
    public void DocumentMapping_Ctor_ThrowsNot()
    {
        // Arrange
        var documentVariant = DocumentVariant.Json;
        var nameValuePath = new ValuePath<string>(documentVariant, "$.Name");
        var ageValuePath = new ValuePath<int>(documentVariant, "$.age");
        var cityValuePath = new ValuePath<string>(documentVariant, "$.Address.City");
        
        // Act
        var action1 = new Action(() => new PropertyMapping<Person, string>(x => x.Name, nameValuePath));
        var action2 = new Action(() => new PropertyMapping<Person, int>(x => x.Age, ageValuePath));
        var action3 = new Action(() => new PropertyMapping<Person, string>(x => x.Address.City, cityValuePath));

        // Assert
        Assert.Null(Record.Exception(action1));
        Assert.Null(Record.Exception(action2));
        Assert.Null(Record.Exception(action3));
    }
    
    [Fact]
    public void DocumentMapping_Ctor_ThrowsWhenValuePathIsNull()
    {
        // Arrange
        var documentVariant = DocumentVariant.Json;
        
        // Act
        var action1 = new Action(() => new PropertyMapping<Person, string>(x => x.Name, null!));
        var action2 = new Action(() => new PropertyMapping<Person, int>(x => x.Age, null!));
        var action3 = new Action(() => new PropertyMapping<Person, string?>(x => x.Address.City, null!));

        // Assert
        Assert.Throws<ArgumentNullException>(action1);
        Assert.Throws<ArgumentNullException>(action2);
        Assert.Throws<ArgumentNullException>(action3);
    }
    
    [Fact]
    public void DocumentMapping_Ctor_ThrowsWhenPropertyExpressionIsNull()
    {
        // Arrange
        var documentVariant = DocumentVariant.Json;
        var nameValuePath = new ValuePath<string>(documentVariant, "$.Name");
        
        // Act
        var action1 = new Action(() => new PropertyMapping<Person, string>(null!, nameValuePath));

        // Assert
        Assert.Throws<ArgumentNullException>(action1);
    }
    
    [Fact]
    public void DocumentMapping_Ctor_ThrowsWhenPropertyExpressionIsNotMemberExpression()
    {
        // Arrange
        var documentVariant = DocumentVariant.Json;
        var nameValuePath = new ValuePath<string>(documentVariant, "$.Name");
        
        // Act
        var action1 = new Action(() => new PropertyMapping<Person, string>(x => "Name", nameValuePath));

        // Assert
        Assert.Throws<ArgumentException>(action1);
    }
    
    
}