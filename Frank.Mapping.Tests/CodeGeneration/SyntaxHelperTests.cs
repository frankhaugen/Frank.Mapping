using System.Reflection;
using FluentAssertions;
using Frank.Mapping.Analyzers;
using Frank.Mapping.Tests.Common.TestingInfrastructure.SourceCode;
using JetBrains.Annotations;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Formatting;
using Microsoft.CodeAnalysis.Simplification;
using Xunit.Abstractions;

namespace Frank.Mapping.Tests.CodeGeneration;

[TestSubject(typeof(SyntaxHelper))]
public class SyntaxHelperTests
{
    private readonly ITestOutputHelper _outputHelper;

    public SyntaxHelperTests(ITestOutputHelper outputHelper)
    {
        _outputHelper = outputHelper;
    }
    
    [Fact]
    public void GenerateMappingInitializer_GeneratesCorrectSyntax()
    {
        // Arrange
        var compilation = CreateCompilation();
        var methodDeclaration = compilation.SyntaxTrees.SelectMany(tree => tree.GetRoot().DescendantNodes())
            .OfType<MethodDeclarationSyntax>()
            .First();
        
        // Act
        var sourceTypeSyntax = methodDeclaration.ParameterList.Parameters[0].Type;
        var targetTypeSyntax = methodDeclaration.ReturnType;
        
        var semanticModel = compilation.GetSemanticModel(methodDeclaration.SyntaxTree);
        var sourceType = semanticModel.GetSymbolInfo(sourceTypeSyntax ?? throw new InvalidOperationException()).Symbol as ITypeSymbol;
        var targetType = semanticModel.GetSymbolInfo(targetTypeSyntax).Symbol as ITypeSymbol;
        var newTargetTypeExprresionSyntax = SyntaxHelper.GenerateMappingInitializer(sourceType, targetType);
        var newMethodReturnStatement = SyntaxFactory.ReturnStatement(newTargetTypeExprresionSyntax);
        var updatedMethod = methodDeclaration.WithBody(SyntaxFactory.Block(newMethodReturnStatement))
            .WithAdditionalAnnotations(Formatter.Annotation, Simplifier.Annotation);
        var returnStatement = updatedMethod.DescendantNodes().OfType<ReturnStatementSyntax>().First();
        var result = returnStatement.NormalizeWhitespace().ToFullString();
        
        // Assert
        var expected = SyntaxFactory.ParseStatement(Expected).As<ReturnStatementSyntax>().NormalizeWhitespace().ToFullString();
        _outputHelper.WriteLine(result);
        _outputHelper.WriteLine(expected);
        result.Should().Be(expected);
    }

    private string Expected = MappingSourceCode.TestResultCode;
    
    private Compilation CreateCompilation()
    {
        var source = MappingSourceCode.TestSourceCode;
        
        return CSharpCompilation.Create("TestCompilation",
            new[] { CSharpSyntaxTree.ParseText(source) },
            new[] { MetadataReference.CreateFromFile(typeof(object).GetTypeInfo().Assembly.Location) },
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
    }
}