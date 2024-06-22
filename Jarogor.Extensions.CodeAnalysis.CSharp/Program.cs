using Jarogor.Extensions.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp;

var path = @"F:\code\projects\jarogor\dotnet\Jarogor.Extensions.CodeAnalysis.CSharp\Jarogor.Extensions.CodeAnalysis.CSharp\HelloWorld.cs";
var tree = CSharpSyntaxTree.ParseText(File.ReadAllText(path));

var root = tree.GetCompilationUnitRoot();

Console.WriteLine("START");

foreach (var attributeResults in root.FindMethodsAttributes("QwertyAttribute"))
{
    Console.WriteLine(string.Join("; ", attributeResults));
}

Console.WriteLine("END");
