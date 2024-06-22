namespace Jarogor.Extensions.CodeAnalysis.CSharp;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
public class QwertyAttribute : Attribute
{
    public string Name { get; }
    public string Aaa { get; set; }

    public string Zzz { get; set; }

    public QwertyAttribute(string name)
    {
        Name = name;
    }
}
