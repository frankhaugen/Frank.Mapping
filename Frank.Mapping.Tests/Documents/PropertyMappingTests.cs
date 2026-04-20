using Frank.Mapping.Documents;
using Frank.Mapping.Documents.Models;
using Frank.Mapping.Documents.Models.Enums;

namespace Frank.Mapping.Tests.Documents;

public class PropertyMappingTests : DocumentsTestBase
{
    [Test]
    public async Task DocumentMapping_Ctor_ThrowsNot()
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
        await Assert.That(action1).ThrowsNothing();
        await Assert.That(action2).ThrowsNothing();
        await Assert.That(action3).ThrowsNothing();
    }
    
    [Test]
    public async Task DocumentMapping_Ctor_ThrowsWhenValuePathIsNull()
    {
        // Arrange
        var documentVariant = DocumentVariant.Json;
        
        // Act
        var action1 = new Action(() => new PropertyMapping<Person, string>(x => x.Name, null!));
        var action2 = new Action(() => new PropertyMapping<Person, int>(x => x.Age, null!));
        var action3 = new Action(() => new PropertyMapping<Person, string?>(x => x.Address.City, null!));

        // Assert
        await Assert.That(action1).ThrowsExactly<ArgumentNullException>();
        await Assert.That(action2).ThrowsExactly<ArgumentNullException>();
        await Assert.That(action3).ThrowsExactly<ArgumentNullException>();
    }
    
    [Test]
    public async Task DocumentMapping_Ctor_ThrowsWhenPropertyExpressionIsNull()
    {
        // Arrange
        var documentVariant = DocumentVariant.Json;
        var nameValuePath = new ValuePath<string>(documentVariant, "$.Name");
        
        // Act
        var action1 = new Action(() => new PropertyMapping<Person, string>(null!, nameValuePath));

        // Assert
        await Assert.That(action1).ThrowsExactly<ArgumentNullException>();
    }
    
    [Test]
    public async Task DocumentMapping_Ctor_ThrowsWhenPropertyExpressionIsNotMemberExpression()
    {
        // Arrange
        var documentVariant = DocumentVariant.Json;
        var nameValuePath = new ValuePath<string>(documentVariant, "$.Name");
        
        // Act
        var action1 = new Action(() => new PropertyMapping<Person, string>(x => "Name", nameValuePath));

        // Assert
        await Assert.That(action1).ThrowsExactly<ArgumentException>();
    }
}