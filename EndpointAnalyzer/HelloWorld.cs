using System;
using System.Collections;
using System.Linq;
using System.Text;
using EndpointAnalyzer;

namespace HelloWorld
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
        }

        [Qwerty("Foo")]
        [Qwerty("Bar")]
        [Qwerty("Foo2", SomeField = "SomeField2")]
        [Qwerty(name: "Foo3", SomeField = "SomeField3")]
        [Qwerty(nameof(HelloWorld.Program.Foo), SomeField = "SomeField4")]
        public void Foo()
        {
            // empty
        }
    }
}
