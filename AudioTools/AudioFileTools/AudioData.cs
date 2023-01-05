using System;
using System.Reflection.PortableExecutable;
using System.Collections;

namespace AudioTools
{
    public interface IAudioData
    {
        public string FileName { get; set; }
        public float[] Left { get; set; } 
        public float[] Right { get; set; }
        public int SampleRate { get; set; }
        public int SampleLength { get; set; }
        public float[] Samples { get; set; }
        public string FileType { get; set; }
    }
}

