namespace Jarogor.Extensions.CodeAnalysis.CSharp
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
        }

        [Qwerty("Foo1")]
        [Qwerty("Foo2", Aaa = "AAA2")]
        [Qwerty("Foo3", Aaa = "AAA3", Zzz = "ZZZ3")]
        [Qwerty(name: "Foo4", Zzz = "ZZZ4", Aaa = "AAA4")]
        [Qwerty( nameof ( Program . Foo ) , Zzz = "ZZZ5")]
        public void Foo()
        {
            // empty
        }
    }
}
