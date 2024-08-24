using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Frank.Mapping.CodeGeneratin;

public static class SyntaxHelper
{
    /// <summary>
    /// Creates an initializer for a property in a mapping context.
    /// </summary>
    public static InitializerExpressionSyntax GetPropertyInitializer(
        ITypeSymbol sourceType, 
        ITypeSymbol targetType, 
        string sourceName, 
        Compilation compilation)
    {
        var initializerExpressions = new List<ExpressionSyntax>();

        foreach (var sourceMember in sourceType.GetMembers().OfType<IPropertySymbol>())
        {
            var targetMember = targetType.GetMembers().OfType<IPropertySymbol>()
                .FirstOrDefault(p => p.Name == sourceMember.Name && p.Type.Equals(sourceMember.Type, SymbolEqualityComparer.Default));

            if (targetMember != null)
            {
                if (IsComplexType(sourceMember.Type, compilation))
                {
                    // Recursive mapping for complex types
                    initializerExpressions.Add(SyntaxFactory.AssignmentExpression(
                        SyntaxKind.SimpleAssignmentExpression,
                        SyntaxFactory.IdentifierName(targetMember.Name),
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.IdentifierName("Map" + sourceMember.Type.Name),
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SingletonSeparatedList(
                                    SyntaxFactory.Argument(SyntaxFactory.IdentifierName(sourceName + "." + sourceMember.Name)))
                            )
                        )
                    ));
                }
                else
                {
                    // Simple assignment for primitive types
                    initializerExpressions.Add(SyntaxFactory.AssignmentExpression(
                        SyntaxKind.SimpleAssignmentExpression,
                        SyntaxFactory.IdentifierName(targetMember.Name),
                        SyntaxFactory.IdentifierName(sourceName + "." + sourceMember.Name)));
                }
            }
            else
            {
                // Default assignment for unmapped properties
                initializerExpressions.Add(SyntaxFactory.AssignmentExpression(
                    SyntaxKind.SimpleAssignmentExpression,
                    SyntaxFactory.IdentifierName(sourceMember.Name),
                    SyntaxFactory.DefaultExpression(SyntaxFactory.ParseTypeName(sourceMember.Type.ToDisplayString()))));
            }
        }

        return SyntaxFactory.InitializerExpression(SyntaxKind.ObjectInitializerExpression, 
            SyntaxFactory.SeparatedList(initializerExpressions));
    }

    /// <summary>
    /// Checks if the type is a complex (non-primitive) type.
    /// </summary>
    public static bool IsComplexType(ITypeSymbol type, Compilation compilation)
    {
        var primitiveTypes = new[] {
            compilation.GetSpecialType(SpecialType.System_Int32),
            compilation.GetSpecialType(SpecialType.System_String),
            compilation.GetSpecialType(SpecialType.System_Boolean),
            compilation.GetSpecialType(SpecialType.System_Double),
            compilation.GetSpecialType(SpecialType.System_DateTime),
            // Add other primitive types as needed
        };

        return !primitiveTypes.Contains(type);
    }

    /// <summary>
    /// Creates a method declaration with the provided body statements.
    /// </summary>
    public static MethodDeclarationSyntax CreateMethod(
        string methodName, 
        TypeSyntax returnType, 
        IEnumerable<StatementSyntax> bodyStatements, 
        bool isAsync = false, 
        params ParameterSyntax[] parameters)
    {
        var method = SyntaxFactory.MethodDeclaration(returnType, methodName)
            .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
            .AddParameterListParameters(parameters)
            .WithBody(SyntaxFactory.Block(bodyStatements));

        if (isAsync)
        {
            method = method.AddModifiers(SyntaxFactory.Token(SyntaxKind.AsyncKeyword));
        }

        return method;
    }
}