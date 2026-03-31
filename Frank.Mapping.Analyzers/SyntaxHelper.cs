using System.Linq.Expressions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Frank.Mapping.Analyzers;

public static class SyntaxHelper
{
    public static ExpressionSyntax GenerateMappingInitializer(ITypeSymbol sourceType, ITypeSymbol targetType, string sourceRootIdentifier = "source")
    {
        // Generate object initializer recursively
        return GenerateObjectInitializer(sourceType, targetType, sourceRootIdentifier);
    }

    private static ObjectCreationExpressionSyntax GenerateObjectInitializer(ITypeSymbol sourceType, ITypeSymbol targetType, string sourceRootIdentifier)
    {
        return SyntaxFactory.ObjectCreationExpression(
                SyntaxFactory.ParseTypeName(targetType.Name)
            ).WithArgumentList(SyntaxFactory.ArgumentList())
            .WithInitializer(GenerateInitializerExpression(sourceType, targetType, sourceRootIdentifier));
    }

    private static InitializerExpressionSyntax GenerateInitializerExpression(ITypeSymbol sourceType, ITypeSymbol targetType, string sourceRootIdentifier)
    {
        var assignments = targetType.GetMembers().OfType<IPropertySymbol>()
            .Select(property => GeneratePropertyAssignment(sourceType, property, sourceRootIdentifier))
            .Where(assignment => assignment != null)
            .ToArray();

        return SyntaxFactory.InitializerExpression(
            SyntaxKind.ObjectInitializerExpression,
            SyntaxFactory.SeparatedList<ExpressionSyntax>(assignments)
        ).WithOpenBraceToken(SyntaxFactory.Token(SyntaxKind.OpenBraceToken)
            .WithTrailingTrivia(SyntaxFactory.LineFeed))
         .WithCloseBraceToken(SyntaxFactory.Token(SyntaxKind.CloseBraceToken)
            .WithLeadingTrivia(SyntaxFactory.LineFeed));
    }

    private static AssignmentExpressionSyntax GeneratePropertyAssignment(ITypeSymbol sourceType, IPropertySymbol targetProperty, string sourceRootIdentifier)
    {
        var sourceProperty = FindMatchingSourceProperty(sourceType, targetProperty);

        if (sourceProperty == null)
        {
            return GenerateDefaultOrNullAssignment(targetProperty);
        }

        var mappingExpression = GenerateMappingExpression(sourceProperty, targetProperty, sourceRootIdentifier);

        return SyntaxFactory.AssignmentExpression(
            SyntaxKind.SimpleAssignmentExpression,
            SyntaxFactory.IdentifierName(targetProperty.Name),
            mappingExpression
        );
    }

    private static IPropertySymbol? FindMatchingSourceProperty(ITypeSymbol sourceType, IPropertySymbol targetProperty)
    {
        return sourceType.GetMembers()
            .OfType<IPropertySymbol>()
            .FirstOrDefault(sourceProperty => sourceProperty.Name == targetProperty.Name);
    }

    private static ExpressionSyntax GenerateMappingExpression(IPropertySymbol sourceProperty, IPropertySymbol targetProperty, string sourceRootIdentifier)
    {
        // Handle direct mapping when the types are the same
        if (SymbolEqualityComparer.Default.Equals(sourceProperty.Type, targetProperty.Type))
        {
            return SyntaxFactory.MemberAccessExpression(
                SyntaxKind.SimpleMemberAccessExpression,
                SyntaxFactory.IdentifierName(sourceRootIdentifier),
                SyntaxFactory.IdentifierName(sourceProperty.Name)
            );
        }

        // Handle recursive mapping for nested types
        if (IsComplexType(sourceProperty.Type) && IsComplexType(targetProperty.Type))
        {
            var nestedSourceRootIdentifier = $"{sourceRootIdentifier}.{sourceProperty.Name}";
            return GenerateMappingInitializer(sourceProperty.Type, targetProperty.Type, nestedSourceRootIdentifier);
        }

        // Fallback to default or null if types are incompatible
        return GenerateDefaultOrNullValue(targetProperty);
    }

    private static AssignmentExpressionSyntax GenerateDefaultOrNullAssignment(IPropertySymbol targetProperty)
    {
        return SyntaxFactory.AssignmentExpression(
            SyntaxKind.SimpleAssignmentExpression,
            SyntaxFactory.IdentifierName(targetProperty.Name),
            GenerateDefaultOrNullValue(targetProperty)
        );
    }

    private static ExpressionSyntax GenerateDefaultOrNullValue(IPropertySymbol targetProperty)
    {
        // If the property is nullable, return `null`
        if (targetProperty.Type.NullableAnnotation == NullableAnnotation.Annotated)
        {
            return SyntaxFactory.LiteralExpression(SyntaxKind.NullLiteralExpression);
        }
        
        // Otherwise, return `default`
        return SyntaxFactory.DefaultExpression(SyntaxFactory.ParseTypeName(targetProperty.Type.Name));
    }

    private static bool IsComplexType(ITypeSymbol type)
    {
        // Determine if the type is a class (complex type)
        return type.TypeKind == TypeKind.Class;
    }
}
