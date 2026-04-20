---
applyTo: "**/*.Tests/**/*.cs,**/*.Test/**/*.cs,**/*Tests.cs,**/*Test.cs"
---

# TUnit Test Writing Guide

This project uses **TUnit** as its test framework. All tests must follow TUnit conventions.

## Test Method Structure

```csharp
[Test]
public async Task MethodName_Scenario_ExpectedResult()
{
    // Arrange
    var expected = "value";

    // Act
    var actual = SystemUnderTest.Method();

    // Assert
    await Assert.That(actual).IsEqualTo(expected);
}
```

Key rules:
- Test methods are decorated with `[Test]` (not `[Fact]` or `[TestMethod]`)
- Test methods **must** return `async Task`
- All assertions use `await Assert.That(actual).Is*(expected)` style

## Assertions Reference

### Equality
```csharp
await Assert.That(actual).IsEqualTo(expected);
await Assert.That(actual).IsNotEqualTo(expected);
```

### Null / Not Null
```csharp
await Assert.That(value).IsNull();
await Assert.That(value).IsNotNull();
```

### Boolean
```csharp
await Assert.That(condition).IsTrue();
await Assert.That(condition).IsFalse();
```

### String
```csharp
await Assert.That(str).IsNotNullOrEmpty();
await Assert.That(str).Contains("substring");
await Assert.That(str).StartsWith("prefix");
await Assert.That(str).EndsWith("suffix");
```

### Collections
```csharp
await Assert.That(collection).Contains(item);
await Assert.That(collection).DoesNotContain(item);
await Assert.That(collection).HasCount(n);
await Assert.That(collection).IsEmpty();
await Assert.That(collection).IsNotEmpty();
```

### Type Checking
```csharp
await Assert.That(obj).IsTypeOf<MyType>();          // exact type
await Assert.That(obj).IsAssignableTo<IMyInterface>(); // assignability
```

### Numeric Range
```csharp
await Assert.That(value).IsGreaterThan(min);
await Assert.That(value).IsGreaterThanOrEqualTo(min);
await Assert.That(value).IsLessThan(max);
await Assert.That(value).IsLessThanOrEqualTo(max);
```

### Equivalence (deep structural equality)
```csharp
await Assert.That(actual).IsEquivalentTo(expected);
```

### Exceptions
```csharp
// Assert throws exact type
await Assert.That(() => action()).ThrowsExactly<ArgumentException>();

// Assert throws exact type and capture for further inspection
var ex = await Assert.That(() => action()).ThrowsExactly<ArgumentException>();
await Assert.That(ex.Message).IsEqualTo("expected message");

// Assert throws T or subtype
await Assert.That(() => action()).Throws<Exception>();

// Assert no exception is thrown
await Assert.That(() => action()).DoesNotThrow();

// For async methods
await Assert.That(async () => await asyncMethod()).ThrowsExactly<InvalidOperationException>();
await Assert.That(async () => await asyncMethod()).DoesNotThrow();
```

## Skipping Tests

```csharp
[Test, Skip("Reason for skipping")]
public async Task SkippedTest()
{
    // ...
}
```

## Parameterized Tests

```csharp
[Test]
[Arguments("hello", 5)]
[Arguments("world!", 6)]
public async Task StringLength_IsCorrect(string input, int expectedLength)
{
    await Assert.That(input.Length).IsEqualTo(expectedLength);
}
```

## Test Output

TUnit captures `Console.WriteLine` output — use it instead of `ITestOutputHelper`:

```csharp
Console.WriteLine($"Result: {actual}");
```

Do NOT inject `ITestOutputHelper` — it does not exist in TUnit.

## No `ITestOutputHelper`

TUnit does NOT use `ITestOutputHelper`. Remove it from all constructors and replace usages with `Console.WriteLine(...)`.

## Test Class Setup

```csharp
// TUnit uses attributes for lifecycle, not constructors
public class MyTests
{
    [Before(Test)]
    public async Task Setup() { /* runs before each test */ }

    [After(Test)]
    public async Task TearDown() { /* runs after each test */ }

    [Before(Class)]
    public static async Task ClassSetup() { /* runs once before all tests in class */ }
}
```

## Packages Required

```xml
<PackageReference Include="TUnit" Version="*" />
<PackageReference Include="Microsoft.NET.Test.Sdk" Version="*" />
```

TUnit includes its own test runner — no separate runner package needed.
