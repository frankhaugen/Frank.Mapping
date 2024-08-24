using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Frank.Mapping.CodeGeneratin;

internal class MappingInterfaceReceiver : ISyntaxReceiver
{
    public List<ClassDeclarationSyntax> CandidateClasses { get; } = new();

    public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
    {
        if (syntaxNode is ClassDeclarationSyntax classDeclaration)
        {
            var hasMappingInterface = classDeclaration.BaseList?.Types
                .Any(t => t.ToString().Contains("IMappingDefinition") || t.ToString().Contains("IAsyncMappingDefinition")) ?? false;

            if (hasMappingInterface)
            {
                CandidateClasses.Add(classDeclaration);
            }
        }
    }
}