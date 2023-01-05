using System;
using System.Reflection.PortableExecutable;
using System.Runtime.InteropServices;

namespace AudioTools
{
    public class WavData : IAudioData
    {
        private string _filename;
        public Dictionary<string, int> HeaderData { get; set; } = new Dictionary<string, int>();
        public byte[] RawHeader { get; set; } = Array.Empty<byte>();
        public string FileName
        {
            get
            {
                return _filename;
            }
            set
            {
                if (String.IsNullOrEmpty(value) == true)
                { throw new Exception("Filename Cannot be null type"); }
                _filename = value;
            }
        }
        public float[] Left { get; set; }
        public float[]? Right { get; set; }
        public int SampleRate { get; set; }
        public int SampleLength { get; set; }
        public float[] Samples { get; set; }
        public string FileType { get; set; } = "Wave";
        public WavData()
        {
            FileName = Console.ReadLine();
            LoadFile();
        }
        public WavData(string filename)
        {
            FileName = filename;
            LoadFile();
        }
        /*Calls function from WAV utilities folder to keep file nice and short
         * LoadFile is protected because this method should only be called once when the object is initialized
         * If Another file needs to load create a new instance of the class */
        protected void LoadFile()
        {
            WavReader.LoadWav(this);
        }
        public void SaveFile(string fileout)
        {
            WavDumper.PackSamplesToWav(this, fileout);
        }
    }
}

