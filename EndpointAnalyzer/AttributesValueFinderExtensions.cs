using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace EndpointAnalyzer;

public static class AttributesValueFinderExtensions
{
    private const string AttributeSuffix = "Attribute";

    public static IEnumerable<string> Find(this CompilationUnitSyntax root, string name)
    {
        return root
            .GetClassDeclarations()
            .GetMethodDeclarations()
            .GetAttributes(name)
            .GetAttributesValues();
    }

    private static IEnumerable<ClassDeclarationSyntax> GetClassDeclarations(this CompilationUnitSyntax self)
    {
        return self
            .DescendantNodes()
            .Where(it => it is ClassDeclarationSyntax)
            .Cast<ClassDeclarationSyntax>();
    }

    private static IEnumerable<MethodDeclarationSyntax> GetMethodDeclarations(this IEnumerable<ClassDeclarationSyntax> self)
    {
        return self
            .SelectMany(it => it.ChildNodes())
            .Where(m => m is MethodDeclarationSyntax)
            .Cast<MethodDeclarationSyntax>()
            .Where(IsPublic);
    }

    private static bool IsPublic(this MethodDeclarationSyntax it)
        => it.Modifiers.Any(t => t.ValueText == "public");

    private static IEnumerable<AttributeSyntax> GetAttributes(this IEnumerable<MethodDeclarationSyntax> self, string name)
    {
        return self
            .SelectMany(it => it.DescendantNodes().OfType<AttributeListSyntax>())
            .SelectMany(it => it.Attributes)
            .Where(a => a.IsAttribute(name));
    }

    private static bool IsAttribute(this AttributeSyntax self, string name)
    {
        var attributeNames = new string[2];
        attributeNames[0] = name;

        if (name.EndsWith(AttributeSuffix))
        {
            var position = name.IndexOf(AttributeSuffix, StringComparison.Ordinal);
            attributeNames[1] = name[..position];
        }
        else
        {
            attributeNames[1] = $"{name}{AttributeSuffix}";
        }

        return attributeNames.Contains(self.Name.GetText().ToString());
    }

    private static IEnumerable<string> GetAttributesValues(this IEnumerable<AttributeSyntax> self)
    {
        return self
            .SelectMany(it => it.ArgumentList?.Arguments.ToList() ?? new List<AttributeArgumentSyntax>())
            .Select(it => it.Expression)
            .Select(GetValue);
    }

    private static string GetValue(this ExpressionSyntax self)
    {
        switch (self)
        {
            case InvocationExpressionSyntax invocation:
                // EXAMPLE: nameof(qwerty.foo.bar)
                //      string[]{ "nameof", "(", "qwerty", ".", "foo", ".", "bar", ")" }
                //                    0      1       2      3     4     5     6     7
                //      count: 8
                //      position: 8 - 6 = 6
                //      position: 6 — bar
                var count = invocation.DescendantTokens().Count() - 2;
                return invocation.DescendantTokens().Skip(count).First().ValueText;
            case LiteralExpressionSyntax literal:
                return literal.Token.ValueText;
            default:
                return string.Empty;
        }
    }
}
