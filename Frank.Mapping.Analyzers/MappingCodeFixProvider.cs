using System.Collections.Immutable;
using System.Composition;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Formatting;
using Microsoft.CodeAnalysis.Simplification;

namespace Frank.Mapping.Analyzers;

[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(MappingCodeFixProvider)), Shared]
public class MappingCodeFixProvider : CodeFixProvider
{
    public sealed override ImmutableArray<string> FixableDiagnosticIds => ["MAP1001"];

    public sealed override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

    public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken);
        var diagnostic = context.Diagnostics[0];
        var diagnosticSpan = diagnostic.Location.SourceSpan;

        var methodDeclaration = root.FindToken(diagnosticSpan.Start).Parent.AncestorsAndSelf().OfType<MethodDeclarationSyntax>().First();

        context.RegisterCodeFix(
            Microsoft.CodeAnalysis.CodeActions.CodeAction.Create(
                title: "Fix Map method",
                createChangedDocument: c => FixMapMethodAsync(context.Document, methodDeclaration, c),
                equivalenceKey: "Fix Map method"),
            diagnostic);
    }

    private async Task<Document> FixMapMethodAsync(Document document, MethodDeclarationSyntax methodDeclaration, CancellationToken cancellationToken)
    {
        // Use Roslyn Syntax APIs to modify the method
        var sourceTypeSyntax = methodDeclaration.ParameterList.Parameters[0].Type;
        var targetTypeSyntax = methodDeclaration.ReturnType;
        
        if (targetTypeSyntax is GenericNameSyntax genericNameSyntax)
        {
            targetTypeSyntax = genericNameSyntax.TypeArgumentList.Arguments[0];
        }
        
        var semanticModel = await document.GetSemanticModelAsync(cancellationToken);
        var sourceType = semanticModel.GetSymbolInfo(sourceTypeSyntax ?? throw new InvalidOperationException()).Symbol as ITypeSymbol;
        var targetType = semanticModel.GetSymbolInfo(targetTypeSyntax).Symbol as ITypeSymbol;
        var newTargetTypeExprresionSyntax = SyntaxHelper.GenerateMappingInitializer(sourceType, targetType);
        var newMethodReturnStatement = SyntaxFactory.ReturnStatement(newTargetTypeExprresionSyntax);
        var updatedMethod = methodDeclaration.WithBody(SyntaxFactory.Block(newMethodReturnStatement))
            .WithAdditionalAnnotations(Formatter.Annotation, Simplifier.Annotation);

        var root = await document.GetSyntaxRootAsync(cancellationToken);
        var newRoot = root.ReplaceNode(methodDeclaration, updatedMethod);

        return document.WithSyntaxRoot(newRoot);
    }
}