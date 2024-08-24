using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;

namespace Frank.Mapping.CodeGeneratin;

[Generator]
public class RecursiveMappingGenerator : ISourceGenerator
{
    public void Initialize(GeneratorInitializationContext context)
    {
        context.RegisterForSyntaxNotifications(() => new MappingInterfaceReceiver());
    }

    public void Execute(GeneratorExecutionContext context)
    {
        if (context.SyntaxReceiver is not MappingInterfaceReceiver receiver) return;

        foreach (var classSyntax in receiver.CandidateClasses)
        {
            var semanticModel = context.Compilation.GetSemanticModel(classSyntax.SyntaxTree);
            var classSymbol = semanticModel.GetDeclaredSymbol(classSyntax) as INamedTypeSymbol;

            if (classSymbol == null) continue;

            foreach (var interfaceType in classSymbol.Interfaces)
            {
                if (IsMappingInterface(interfaceType))
                {
                    var sourceType = interfaceType.TypeArguments[0];
                    var targetType = interfaceType.TypeArguments[1];

                    var generatedCode = GenerateMappingCode(classSymbol, interfaceType, sourceType, targetType, context.Compilation);
                    context.AddSource($"{classSymbol.Name}_GeneratedMapper.cs", SourceText.From(generatedCode, Encoding.UTF8));
                }
            }
        }
    }

    private bool IsMappingInterface(INamedTypeSymbol interfaceSymbol)
    {
        return interfaceSymbol.Name == "IAsyncMappingDefinition" || interfaceSymbol.Name == "IMappingDefinition";
    }

    private string GenerateMappingCode(INamedTypeSymbol classSymbol, INamedTypeSymbol interfaceSymbol, ITypeSymbol sourceType, ITypeSymbol targetType, Compilation compilation)
    {
        var isAsync = interfaceSymbol.Name == "IAsyncMappingDefinition";

        var sb = new StringBuilder();
        sb.AppendLine($"public partial class {classSymbol.Name}");
        sb.AppendLine("{");

        // Method signature
        if (isAsync)
        {
            sb.AppendLine("    /// <summary>");
            sb.AppendLine("    /// Maps an object of type TFrom to an object of type TTo asynchronously.");
            sb.AppendLine("    /// </summary>");
            sb.AppendLine("    /// <param name=\"source\">The object to be mapped.</param>");
            sb.AppendLine("    /// <returns>A task representing the asynchronous operation, which will eventually contain the mapped object of type TTo.</returns>");
            sb.AppendLine($"    public async Task<{targetType}> MapAsync({sourceType} source)");
        }
        else
        {
            sb.AppendLine("    /// <summary>");
            sb.AppendLine("    /// Maps an object of type TFrom to an object of type TTo.");
            sb.AppendLine("    /// </summary>");
            sb.AppendLine($"    /// <param name=\"source\">The object to be mapped.</param>");
            sb.AppendLine($"    /// <returns>The mapped object of type {targetType}.</returns>");
            sb.AppendLine($"    public {targetType} Map({sourceType} source)");
        }

        sb.AppendLine("    {");

        // Generate initializer block using SyntaxHelper
        var propertyInitializers = SyntaxHelper.GetPropertyInitializer(sourceType, targetType, "source", compilation);

        sb.AppendLine("        return new " + targetType.ToDisplayString() + " ");
        sb.AppendLine("        {");

        foreach (var expression in propertyInitializers.Expressions)
        {
            sb.AppendLine("            " + expression.ToString() + ",");
        }

        sb.AppendLine("        };");
        sb.AppendLine("    }");
        sb.AppendLine("}");

        return sb.ToString();
    }
}