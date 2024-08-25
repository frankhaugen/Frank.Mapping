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
        
        // If class don't implements IMappingDefinition or IAsyncMappingDefinition, skip
        var parent = methodDeclaration.Parent;
        var parentClass = parent as Microsoft.CodeAnalysis.CSharp.Syntax.ClassDeclarationSyntax;
        var parentClassSymbol = context.SemanticModel.GetDeclaredSymbol(parentClass);
        var parentClassBaseList = parentClass.BaseList;
        
        if (parentClassBaseList == null || parentClassBaseList.Types.Count == 0)
        {
            return;
        }
        
        var implementsIMappingDefinition = false;
        
        foreach (var baseType in parentClassBaseList.Types)
        {
            var baseTypeSymbol = context.SemanticModel.GetTypeInfo(baseType.Type).Type;
            if (baseTypeSymbol == null)
            {
                continue;
            }
            
            if (baseTypeSymbol.Name == "IMappingDefinition" || baseTypeSymbol.Name == "IAsyncMappingDefinition")
            {
                implementsIMappingDefinition = true;
                break;
            }
        }

        // Detect incomplete Map methods and raise diagnostic
        if (implementsIMappingDefinition && (methodDeclaration.Identifier.Text == "Map" || methodDeclaration.Identifier.Text == "MapAsync") && methodDeclaration.ParameterList.Parameters.Count == 1 && methodDeclaration.Body?.Statements.Count == 0)
        {
            // Perform further checks here (e.g., validate the method body)
            var diagnostic = Diagnostic.Create(Rule, methodDeclaration.GetLocation(), "SourceType", "TargetType");
            context.ReportDiagnostic(diagnostic);
        }
    }
}