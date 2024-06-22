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
          
                  [Qwerty("1")]
                  [ Qwerty("2",Foo = "2")]
                  [QwertyAttribute  ("3", Foo = "3", Bar = "3")]
                  [ Qwerty(name: "4", Bar = "4", Foo="4" ) ]
                  [Qwerty  ( nameof( HelloWorld . Program . Do ), Bar = "5")]
                  public void Do()
                  {
                  }
              }
          }
          """;

    private static readonly List<List<AttributeInfo>> Expected = new()
    {
        new List<AttributeInfo> { new(0, string.Empty, "1") },
        new List<AttributeInfo> { new(0, string.Empty, "2"), new(1, "Foo", "2") },
        new List<AttributeInfo> { new(0, string.Empty, "3"), new(1, "Foo", "3"), new(2, "Bar", "3") },
        new List<AttributeInfo> { new(0, "name", "4"), new(1, "Bar", "4"), new(2, "Foo", "4") },
        new List<AttributeInfo> { new(0, string.Empty, "Do"), new(1, "Bar", "5") },
    };

    private CompilationUnitSyntax? _root;

    [SetUp]
    public void SetUp()
    {
        _root = CSharpSyntaxTree.ParseText(CSharpCode).GetCompilationUnitRoot();
    }

    [TestCase(" Qwerty ")]
    [TestCase(" QwertyAttribute ")]
    public void TestFindAttributeValues(string attrName)
    {
        var attributes = _root!.FindMethodsAttributes(attrName).ToList();
        Assert.AreEqual(Expected.Count, attributes.Count);

        for (var a = 0; a < attributes.Count; a++)
        {
            Assert.NotNull(attributes[a]);

            var args = attributes[a]!.ToList();
            Assert.AreEqual(Expected[a].Count, args.Count);

            for (var b = 0; b < args.Count; b++)
            {
                Assert.AreEqual(Expected[a][b], args[b]);
            }
        }
    }
}
