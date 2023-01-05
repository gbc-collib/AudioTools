using System;
using System.Diagnostics;
using System.IO.Enumeration;
using AudioTools.EditingTools;

namespace AudioTools
{
    internal class Program
    {
        static void Main()
        {
            Console.WriteLine("Hello World!");
            WavData MusicFile = new("/Users/collinstasisk/Documents/GitHub/AudioTools/AudioTools/TestAudioFiles/PinkPanther30.wav");
            Console.WriteLine("Loading Done");
            //Reverberator.SchroederReverb(MusicFile, 150, 0.8f, 100);
            //Console.WriteLine("Reverbing done");
            Distortion.OverdriveDistortion(MusicFile, 10, 800, 100);
            MusicFile.SaveFile("/Users/collinstasisk/Documents/GitHub/AudioTools/AudioTools/Reverbed.wav");
            Console.WriteLine("Saving done");
        }
    }
}