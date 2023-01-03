using System;
using System.Reflection.PortableExecutable;
using System.Collections;

namespace AudioTools
{
	public class AudioData
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
        public Dictionary<string, int> HeaderData { get; set; } = new Dictionary<string, int>();
        public AudioData()
		{
            Console.WriteLine("Write File name");
            FileName = Console.ReadLine();
            AudioFileHandler.ReadWavToFloat(this);
            Samples = Left;
            //Reverberator.Reverb(this, );

        }
	}
}

