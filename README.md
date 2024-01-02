# Frank.Mapping
A simple mapping library

___
[![GitHub License](https://img.shields.io/github/license/frankhaugen/Frank.Mapping)](LICENSE)
[![NuGet](https://img.shields.io/nuget/v/Frank.Mapping.svg)](https://www.nuget.org/packages/Frank.Mapping)
[![NuGet](https://img.shields.io/nuget/dt/Frank.Mapping.svg)](https://www.nuget.org/packages/Frank.Mapping)

![GitHub contributors](https://img.shields.io/github/contributors/frankhaugen/Frank.Mapping)
![GitHub Release Date - Published_At](https://img.shields.io/github/release-date/frankhaugen/Frank.Mapping)
![GitHub last commit](https://img.shields.io/github/last-commit/frankhaugen/Frank.Mapping)
![GitHub commit activity](https://img.shields.io/github/commit-activity/m/frankhaugen/Frank.Mapping)
![GitHub pull requests](https://img.shields.io/github/issues-pr/frankhaugen/Frank.Mapping)
![GitHub issues](https://img.shields.io/github/issues/frankhaugen/Frank.Mapping)
![GitHub closed issues](https://img.shields.io/github/issues-closed/frankhaugen/Frank.Mapping)
___

## Installation

### NuGet

```powershell
Install-Package Frank.Mapping
```

## Usage

### Create a mapping

```csharp
private class MyMappingDefinition : IMappingDefinition<From, To>
{
    public To Map(From from)
    {
        return new()
        {
            Id = from.Id,
            Name = $"{from.FirstName} {from.LastName}"
        };
    }
}
```

### Register the mapping

```csharp
var serviceProvider = new ServiceCollection()
    .AddMappingDefinition<From, To, MyMappingDefinition>()
    .BuildServiceProvider();
```

### Consuming the mapping through dependency injection

```csharp
// Option 1: Directly
public class MyService
{
    private readonly IMappingDefinition<From, To> _mappingDefinition;
    
    public MyService(IMappingDefinition<From, To> mappingDefinition)
    {
        _mappingDefinition = mappingDefinition;
    }
    
    public void DoSomething(From from)
    {
        var to = _mappingDefinition.Map(from);
        
        // Do something with the mapped object
    }
}

// Option 2: Through a mappingprovider
public class MyService
{
    private readonly IMappingProvider _mappingProvider;
    
    public MyService(IMappingProvider mappingProvider)
    {
        _mappingProvider = mappingProvider;
    }
    
    public void DoSomething(From from)
    {
        var to = _mappingProvider.Map<From, To>(from);
        
        // Do something with the mapped object
    }
}
```