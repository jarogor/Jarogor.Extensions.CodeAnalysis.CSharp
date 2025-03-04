using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Jarogor.Extensions.CodeAnalysis.CSharp;

/// <summary>
///     Метод расширения поиска атрибутов методов.
/// </summary>
public static class MethodAttributesExtensions
{
    private const string AttributeSuffix = "Attribute";

    /// <summary>
    ///     Поиск атрибутов методов по имени
    /// </summary>
    /// <param name="root">Объект CompilationUnitSyntax</param>
    /// <param name="name">Имя атрибута</param>
    /// <returns>
    ///     Двухуровневая коллекция найденных атрибутов. Внутри каждого элемента атрибута коллекция AttributeInfo —
    ///     информация о параметрах.
    /// </returns>
    public static IEnumerable<IEnumerable<AttributeInfo>?> FindMethodsAttributes(this CompilationUnitSyntax root, string name)
    {
        return root
            .GetClassDeclarationSyntaxList()
            .GetMethodDeclarationSyntaxList()
            .GetAttributeSyntaxList(name.Trim())
            .GetAttributeInfoList();
    }

    private static IEnumerable<ClassDeclarationSyntax> GetClassDeclarationSyntaxList(this SyntaxNode self)
    {
        return self
            .DescendantNodes()
            .Where(it => it is ClassDeclarationSyntax)
            .Cast<ClassDeclarationSyntax>();
    }

    private static IEnumerable<MethodDeclarationSyntax> GetMethodDeclarationSyntaxList(this IEnumerable<ClassDeclarationSyntax> self)
    {
        return self
            .SelectMany(it => it.ChildNodes())
            .Where(m => m is MethodDeclarationSyntax)
            .Cast<MethodDeclarationSyntax>()
            .Where(IsPublic);
    }

    private static bool IsPublic(this MethodDeclarationSyntax it)
    {
        return it.Modifiers.Any(t => t.IsKind(SyntaxKind.PublicKeyword));
    }

    private static IEnumerable<AttributeSyntax> GetAttributeSyntaxList(this IEnumerable<MethodDeclarationSyntax> self, string name)
    {
        return self
            .SelectMany(it => it.DescendantNodes().OfType<AttributeListSyntax>())
            .SelectMany(it => it.Attributes)
            .Where(a => a.IsAttribute(name));
    }

    private static bool IsAttribute(this AttributeSyntax self, string name)
    {
        string[] attributeNames = new string[2];
        attributeNames[0] = name;

        if (name.EndsWith(AttributeSuffix))
        {
            attributeNames[1] = name.Substring(0, name.Length - AttributeSuffix.Length);
        }
        else
        {
            attributeNames[1] = $"{name}{AttributeSuffix}";
        }

        return attributeNames.Contains(self.Name.NormalizeWhitespace().GetText().ToString());
    }

    private static IEnumerable<IEnumerable<AttributeInfo>?> GetAttributeInfoList(this IEnumerable<AttributeSyntax> self)
    {
        return self
            .Select(it => it.ArgumentList)
            .Select(it
                => (it?.Arguments.ToList() ?? new List<AttributeArgumentSyntax>())
                .Select((argument, index) => new AttributeInfo(index, argument.GetName(), argument.Expression.GetValue()))
            );
    }

    private static string GetName(this AttributeArgumentSyntax self)
    {
        return self.NameColon?.Name.Identifier.ValueText ?? self.NameEquals?.Name.Identifier.ValueText ?? string.Empty;
    }

    private static string GetValue(this ExpressionSyntax self)
    {
        return self switch
        {
            // EXAMPLE: nameof(qwerty.foo.bar) -> qwerty.foo.bar -> last: bar
            InvocationExpressionSyntax invocation => invocation.DescendantNodes().Last().GetLastToken().ValueText,
            LiteralExpressionSyntax literal => literal.Token.ValueText,
            _ => string.Empty,
        };
    }
}
