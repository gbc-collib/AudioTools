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
            AudioData MusicFile = new();
            AudioFileHandler.PackFloatToWav("Reverbed.wav", MusicFile);
        }
    }
    
    
}