using System.Collections.Immutable;
using System.Reflection;
using Frank.Mapping.Analyzers;
using Frank.Mapping.Documents.Models;
using Frank.Mapping.Documents.Models.Enums;
using Frank.Mapping.Tests.Common.TestingInfrastructure;
using Frank.Mapping.Tests.Common.TestingInfrastructure.SourceCode;
using Frank.Mapping.Tests.Documents;
using JetBrains.Annotations;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Formatting;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Text;

namespace Frank.Mapping.Tests.Analyzers;

[TestSubject(typeof(MappingAnalyzer))]
public class MappingAnalyzerTests
{
    [Test]
    public async Task Test()
    {
        var code = MappingSourceCode.TestSourceCode;
        await new CSharpAnalyzerTest<MappingAnalyzer, DefaultVerifier>
        {
            TestCode = code,
            ReferenceAssemblies = ReferenceAssemblies.Default.WithAssemblies([ 
                typeof(IMappingDefinition<string, string>).Assembly.Location
            ]),
            SolutionTransforms =
            {
                (solution, projectId) =>
                {
                    var parseOptions = CSharpParseOptions.Default.WithDocumentationMode(DocumentationMode.Diagnose);
                    return solution.WithProjectParseOptions(projectId, parseOptions);
                }
            },
            ExpectedDiagnostics = 
            {
                DiagnosticResult.CompilerError("CS0246").WithSpan(5, 27, 5, 84).WithArguments("IMappingDefinition<,>"),
                DiagnosticResult.CompilerWarning("MAP1001").WithSpan(7, 5, 9, 6).WithArguments("SourceType", "TargetType"),
                DiagnosticResult.CompilerError("CS0161").WithSpan(7, 33, 7, 36).WithArguments("Frank.Mapping.Tests.TestingInfrastructure.TestMapper.Map(Frank.Mapping.Tests.TestingInfrastructure.TestSourceClass)"),
                DiagnosticResult.CompilerError("CS0246").WithSpan(24, 12, 24, 16).WithArguments("Guid")
            }
        }.RunAsync();
        
    }
    
    [Test, Skip("This test is overly sensitive to the environment it runs in and is not reliable")]
    public async Task TestFixer()
    {
        var code = MappingSourceCode.TestSourceCode;
        var fixCode = MappingSourceCode.CompleteResultCode;

        // Normalize line endings for both the test code and the expected fixed code
        code = await NormalizeLineEndings(code);
        fixCode = await NormalizeLineEndings(fixCode);
    
        await new CSharpCodeFixTest<MappingAnalyzer, MappingCodeFixProvider, DefaultVerifier>
        {
            TestCode = code,
            FixedCode = fixCode,
            ReferenceAssemblies = ReferenceAssemblies.Default,
            ExpectedDiagnostics =
            {
                DiagnosticResult.CompilerError("CS0246").WithSpan(5, 27, 5, 84).WithArguments("IMappingDefinition<,>"),
                DiagnosticResult.CompilerWarning("MAP1001").WithSpan(7, 5, 9, 6).WithArguments("SourceType", "TargetType"),
                DiagnosticResult.CompilerError("CS0161").WithSpan(7, 33, 7, 36).WithArguments("Frank.Mapping.Tests.TestingInfrastructure.TestMapper.Map(Frank.Mapping.Tests.TestingInfrastructure.TestSourceClass)"),
                DiagnosticResult.CompilerError("CS0246").WithSpan(24, 12, 24, 16).WithArguments("Guid")
            }
        }.RunAsync();
    }

    private static async Task<string> NormalizeLineEndings(string input)
    {
        input = input.Replace("\r\n", "\n");
        input = input.Replace("\n", Environment.NewLine);
        
        var workspace = new AdhocWorkspace();
        var project = workspace.AddProject("TestProject", LanguageNames.CSharp);
        project = project.AddDocument("Test.cs", SourceText.From(input)).Project;
        var document = project.Documents.First();
        
        var formattedDocument = await Formatter.FormatAsync(document);
        var formattedText = await formattedDocument.GetTextAsync();
            
        return formattedText.ToString();
    }
}