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
                  public void Main(string[] args)
                  {
                      Console.WriteLine("Hello, World!");
                  }
          
                  [Qwerty("1")]
                  [ Qwerty("2",Foo = "2")]
                  [QwertyAttribute  ("3", Foo = "3", Bar = "3")]
                  public void Do()
                  {
                  }
              }
          }

          namespace HelloWorld2
          {
              public class Program
              {
                  [ Qwerty(name: "4", Bar = "4", Foo="4" ) ]
                  [Qwerty  ( nameof( HelloWorld2 . Program . Do2 ), Bar = "5")]
                  public void Do2()
                  {
                  }
              }
          }
          """;

    private static readonly List<List<AttributeInfo>> Expected =
    [
        [
            new AttributeInfo(0, string.Empty, "1"),
        ],
        [
            new AttributeInfo(0, string.Empty, "2"),
            new AttributeInfo(1, "Foo", "2"),
        ],
        [
            new AttributeInfo(0, string.Empty, "3"),
            new AttributeInfo(1, "Foo", "3"),
            new AttributeInfo(2, "Bar", "3"),
        ],
        [
            new AttributeInfo(0, "name", "4"),
            new AttributeInfo(1, "Bar", "4"),
            new AttributeInfo(2, "Foo", "4"),
        ],
        [
            new AttributeInfo(0, string.Empty, "Do2"),
            new AttributeInfo(1, "Bar", "5"),
        ],
    ];

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
        List<IEnumerable<AttributeInfo>> attributes = _root!.FindMethodsAttributes(attrName).ToList();
        Assert.That(attributes, Has.Count.EqualTo(Expected.Count));

        for (int a = 0; a < attributes.Count; a++)
        {
            Assert.That(attributes[a], Is.Not.Null);

            List<AttributeInfo> args = attributes[a]!.ToList();
            Assert.That(args, Has.Count.EqualTo(Expected[a].Count));

            for (int b = 0; b < args.Count; b++)
            {
                Assert.That(args[b], Is.EqualTo(Expected[a][b]));
            }
        }
    }
}