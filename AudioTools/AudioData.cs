using System;
using System.Reflection.PortableExecutable;
using System.Collections;

namespace AudioTools
{
    abstract public class AudioData
    {
        private string _filename = string.Empty;
        public string FileName
        {
            get
            {
                return _filename;
            }

            set
            {
                if (string.IsNullOrEmpty(value))
                { throw new ArgumentNullException("Filename cannot be null or empty"); }
                _filename = value;
            }
        }
        public float[] Left { get; set; } = Array.Empty<float>();
        public float[] Right { get; set; } = Array.Empty<float>();
        public int SampleRate { get; set; } = 0;
        public int SampleLength { get; set; }
        public float[] Samples { get; set; } = Array.Empty<float>();
        public byte[] RawHeader { get; set; } = Array.Empty<byte>();
        public string FileType { get; set; } = String.Empty;

        public Dictionary<string, int> HeaderData { get; set; } = new Dictionary<string, int>();

        public AudioData() : this(Console.ReadLine())
        {
            Console.WriteLine("Enter Filename:");
        }
        public AudioData(string filename)
        {
            FileName = filename;
            if (FileType == String.Empty)
            {
                return;
            }
            GetFileForm();
        }
        public void GetFileForm()
        {
            switch(Path.GetExtension(FileName))
            {
                case ".wav":
                    FileType = "Wave";

                    return;
                default:
                    throw new NotImplementedException();
            }
        }

    }
}

