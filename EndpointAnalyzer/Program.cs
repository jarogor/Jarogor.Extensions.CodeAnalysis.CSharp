using EndpointAnalyzer;
using Microsoft.CodeAnalysis.CSharp;

var tree = CSharpSyntaxTree.ParseText(File.ReadAllText(@"F:\code\dotnet\EndpointAnalyzer\EndpointAnalyzer\HelloWorld.cs"));

var root = tree.GetCompilationUnitRoot();
foreach (var value in root.Find("QwertyAttribute"))
{
    Console.WriteLine(value);
}
