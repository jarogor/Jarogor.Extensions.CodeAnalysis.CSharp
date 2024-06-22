using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Jarogor.Extensions.CodeAnalysis.CSharp.Test;

public class MethodAttributesExtensionsTests
{
    private const string CSharpCode
        = """
          using System;
          using System.Collections;
          using System.Linq;
          using System.Text;

          namespace HelloWorld
          {
              public class Program
              {
                  static void Main(string[] args)
                  {
                      Console.WriteLine("Hello, World!");
                  }
          
                  [Qwerty("Foo1")]
                  [Qwerty("Foo2", Aaa = "AAA2")]
                  [QwertyAttribute("Foo3", Aaa = "AAA3", Zzz = "ZZZ3")]
                  [Qwerty(name: "Foo4", Zzz = "ZZZ4", Aaa = "AAA4")]
                  [Qwerty(nameof(HelloWorld.Program.Foo), Zzz = "ZZZ5")]
                  public void Foo()
                  {
                  }
              }
          }
          """;

    private static readonly List<List<AttributeInfo>?> Expected = new()
    {
        new List<AttributeInfo> { new(0, string.Empty, "Foo1") },
        new List<AttributeInfo> { new(0, string.Empty, "Foo2"), new(1, "Aaa", "AAA2") },
        new List<AttributeInfo> { new(0, string.Empty, "Foo3"), new(1, "Aaa", "AAA3"), new(2, "Zzz", "ZZZ3") },
        new List<AttributeInfo> { new(0, "name", "Foo4"), new(1, "Zzz", "ZZZ4"), new(2, "Aaa", "AAA4") },
        new List<AttributeInfo> { new(0, string.Empty, "Foo"), new(1, "Zzz", "ZZZ5") },
    };

    private CompilationUnitSyntax? _root;

    [SetUp]
    public void SetUp()
    {
        _root = CSharpSyntaxTree.ParseText(CSharpCode).GetCompilationUnitRoot();
    }

    [TestCase("Qwerty")]
    [TestCase("QwertyAttribute")]
    public void TestFindAttributeValues(string attrName)
    {
        var attributes = _root!.FindMethodsAttributes(attrName).ToList();
        Assert.AreEqual(Expected.Count, attributes.Count);

        for (var i = 0; i < attributes.Count; i++)
        {
            var args = attributes[i]?.ToList() ?? new List<AttributeInfo>();
            Assert.AreEqual(Expected[i]!.Count, args.Count);

            for (var j = 0; j < args.Count; j++)
            {
                Assert.AreEqual(Expected[i]![j].Name, args[j].Name);
                Assert.AreEqual(Expected[i]![j].Value, args[j].Value);
                Assert.AreEqual(Expected[i]![j].Position, args[j].Position);
            }
        }
    }
}
