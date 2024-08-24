using Frank.Mapping.Documents;
using Frank.Mapping.Documents.Extensions;
using Frank.Mapping.Documents.Path;
using Frank.Mapping.Tests.Documents;
using JetBrains.Annotations;
using Xunit.Abstractions;

namespace Frank.Mapping.Tests.Extensions;

[TestSubject(typeof(JsonPathDefinitionExtensions))]
public class JsonPathDefinitionExtensionsTests : DocumentsTestBase
{
    /// <inheritdoc />
    public JsonPathDefinitionExtensionsTests(ITestOutputHelper outputHelper) : base(outputHelper)
    {
    }
}