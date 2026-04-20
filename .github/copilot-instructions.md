# Copilot Instructions for Frank.Mapping

## Build & Test Commands

```bash
# Build the entire solution
dotnet build Frank.Mapping.slnx --configuration Release

# Run all tests
dotnet test Frank.Mapping.slnx

# Run a single test class
dotnet test Frank.Mapping.Tests/Frank.Mapping.Tests.csproj --filter "FullyQualifiedName~MappingProviderTests"

# Run a single test method
dotnet test Frank.Mapping.Tests/Frank.Mapping.Tests.csproj --filter "FullyQualifiedName~MappingProviderTests.Map_ReturnsCorrectResult"
```

## Architecture

The solution has four main projects:

- **`Frank.Mapping`** — Core library. Provides the DI-based mapping abstraction: `IMappingDefinition<TSource, TDestination>` (sync), `IAsyncMappingDefinition<TSource, TDestination>` (async), and `IMappingProvider` (facade). `MappingProvider` resolves definitions from `IServiceProvider` at call time. `SimpleMapping<T1, T2>` is an internal adapter wrapping a `Func<T1, T2>`.

- **`Frank.Mapping.Analyzers`** — Roslyn analyzer + code fix. Diagnostic `MAP1001` warns when a class implementing `IMappingDefinition` or `IAsyncMappingDefinition` has an empty `Map`/`MapAsync` body. `MappingCodeFixProvider` generates a property-by-property object initializer using `SyntaxHelper`, which also handles nested complex types recursively.

- **`Frank.Mapping.Documents`** — Separate package for mapping structured text documents (JSON/XML) to objects. Uses `JsonPath.Net` and `SafeFluentXPath`. `Document` auto-detects format via `DocumentVariantHelper` and supports extraction via `ValuePath` lists or `DocumentMapping<T>` definitions with `PropertyMapping` entries.

- **`Frank.Mapping.Tests`** — xUnit test project that references all three libraries above. Tests for the Roslyn analyzer use `CSharpAnalyzerTest<MappingAnalyzer, DefaultVerifier>` from `Microsoft.CodeAnalysis.Analyzer.Testing`. `Frank.Mapping.Tests.Common` holds shared test fixtures and source-code strings used by analyzer tests.

## Key Conventions

### Registration
All three `ServiceCollectionExtensions` overloads auto-register `IMappingProvider` as a singleton if not already present:
```csharp
services.AddMappingDefinition<TFrom, TTo, TMapping>();       // class-based
services.AddAsyncMappingDefinition<TFrom, TTo, TMapping>();  // async class-based
services.AddSimpleMapping<TFrom, TTo>(func);                 // lambda-based
```
Mappings are always registered as **singletons**.

### Target Framework & Language
- Target: `net10.0` (set in `Directory.Build.props`)
- `LangVersion=latest`, `Nullable=enable`, `TreatWarningsAsErrors=true` globally
- Test projects suppress many CA/xUnit warnings and disable NET analyzers

### Test Style
- xUnit with `ITestOutputHelper` injected via constructor for diagnostic output
- FluentAssertions used alongside `Assert.*` (both styles coexist)
- Test classes are annotated with `[TestSubject(typeof(...))]` from `JetBrains.Annotations`
- Test fixtures (reusable source/destination types) live in `Frank.Mapping.Tests.Common`
- The `TestFixer` analyzer test is permanently skipped (`Skip = "..."`) due to environment sensitivity

### Analyzer Tests
When writing new Roslyn analyzer tests, use `CSharpAnalyzerTest<TAnalyzer, DefaultVerifier>` and declare expected diagnostics with both compiler errors (`CS*`) and custom warnings (`MAP*`). Source code strings for these tests live in `MappingSourceCode` (in `Frank.Mapping.Tests.Common/TestingInfrastructure/SourceCode/`).

### Artifacts Output
Build artifacts go to `./artifacts/` (configured via `UseArtifactsOutput` in `Directory.Build.props`). Do not reference `bin/` or `obj/` paths directly.
