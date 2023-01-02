using System;
using System.Diagnostics;
using System.IO.Enumeration;

namespace AudioTools
{
    internal class Program
    {
        static void Main()
        {
            Console.WriteLine("Hello World!");
            AudioFileHandler Test1 = new();
            Console.WriteLine($"Left: {Test1.Left}");
            Console.WriteLine($"Right: {Test1.Right}");
        }
    }
    
    
}