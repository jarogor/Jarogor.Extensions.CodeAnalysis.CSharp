using EndpointAnalyzer;
using Microsoft.CodeAnalysis.CSharp;

namespace EndpointAnalyzerTest;

public class AttributesValueFinderTests
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
          
                  [QwertyFooBar("Foo-0")]
                  [QwertyFooBarAttribute("Foo-1")]
                  [QwertyFooBar("Foo-2", SomeField = "Bar-2")]
                  [QwertyFooBar(name: "Foo-3", SomeField = "Bar-3")]
                  [QwertyFooBar(nameof(HelloWorld.Program.Foo), SomeField = "Bar-4")]
                  public void Foo()
                  {
                  }
              }
          }
          """;

    private static readonly string[] Expected = {
        "Foo-0",
        "Foo-1",
        "Foo-2",
        "Bar-2",
        "Foo-3",
        "Bar-3",
        "Foo",
        "Bar-4",
    };

    [Test]
    public void TestFindAttributeValues()
    {
        var tree = CSharpSyntaxTree.ParseText(CSharpCode);

        var root = tree.GetCompilationUnitRoot();
        var found = root.Find("QwertyFooBarAttribute").ToList();
        
        for (var index = 0; index < found.Count; index++)
        {
            Assert.AreEqual(Expected[index], found[index]);
        }
    }
}
