namespace AudioTools.AudioFileTools
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
        public bool LoadFile();
        public void SaveFile(string fileOut);
        public bool ToBytes(out byte[] samplesAsByteArray);
    }
}

