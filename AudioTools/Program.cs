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
            WavData MusicFile = new("/Users/collinstasisk/Documents/GitHub/AudioTools/AudioTools/PinkPanther30.wav");
            Reverberator.Reverb(MusicFile, 152, 0.15f, 100);
            MusicFile.PackFloatToWav("/Users/collinstasisk/Documents/GitHub/AudioTools/AudioTools/Reverbed.wav");
        }
    }
    
    
}