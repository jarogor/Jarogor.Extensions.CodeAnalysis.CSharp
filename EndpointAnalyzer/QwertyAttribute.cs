namespace EndpointAnalyzer;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
public class QwertyAttribute : Attribute
{
    public string Name { get; }
    public string SomeField { get; set; }

    public QwertyAttribute(string name)
    {
        Name = name;
    }
}
