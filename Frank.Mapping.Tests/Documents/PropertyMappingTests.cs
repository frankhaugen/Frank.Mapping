using FluentAssertions;
using Frank.Mapping.Documents;
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
        action1.Should().NotThrow();
        action2.Should().NotThrow();
        action3.Should().NotThrow();
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
        action1.Should().Throw<ArgumentNullException>();
        action2.Should().Throw<ArgumentNullException>();
        action3.Should().Throw<ArgumentNullException>();
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
        action1.Should().Throw<ArgumentNullException>();
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
        action1.Should().Throw<ArgumentException>();
    }
    
    
}