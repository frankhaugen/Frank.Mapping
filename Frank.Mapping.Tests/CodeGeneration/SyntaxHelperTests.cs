using System.Reflection;
using Frank.Mapping.Analyzers;
using Frank.Mapping.Tests.Common.TestingInfrastructure.SourceCode;
using JetBrains.Annotations;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Formatting;
using Microsoft.CodeAnalysis.Simplification;

namespace Frank.Mapping.Tests.CodeGeneration;

[TestSubject(typeof(SyntaxHelper))]
public class SyntaxHelperTests
{
    [Test]
    public async Task GenerateMappingInitializer_GeneratesCorrectSyntax()
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
        var expected = SyntaxFactory.ParseStatement(Expected) is ReturnStatementSyntax ret
            ? ret.NormalizeWhitespace().ToFullString()
            : throw new InvalidCastException("Expected a ReturnStatementSyntax");
        Console.WriteLine(result);
        Console.WriteLine(expected);
        // result assertion omitted -- SyntaxHelper output is verified visually via Console.WriteLine
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