using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Frank.Mapping.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class MappingAnalyzer : DiagnosticAnalyzer
{
    private static DiagnosticDescriptor Rule = new DiagnosticDescriptor(
        id: "MAP1001",
        title: "Incomplete Map Method",
        messageFormat: "The Map method should properly map properties between {0} and {1}",
        category: "Mapping",
        DiagnosticSeverity.Warning,
        isEnabledByDefault: true);

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        context.RegisterSyntaxNodeAction(AnalyzeSyntax, Microsoft.CodeAnalysis.CSharp.SyntaxKind.MethodDeclaration);
    }

    private void AnalyzeSyntax(SyntaxNodeAnalysisContext context)
    {
        var methodDeclaration = (Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax)context.Node;

        // Detect incomplete Map methods and raise diagnostic
        if (methodDeclaration.Identifier.Text == "Map" && methodDeclaration.ParameterList.Parameters.Count == 1 && methodDeclaration.Body?.Statements.Count == 0)
        {
            // Perform further checks here (e.g., validate the method body)
            var diagnostic = Diagnostic.Create(Rule, methodDeclaration.GetLocation(), "SourceType", "TargetType");
            context.ReportDiagnostic(diagnostic);
        }
    }
}