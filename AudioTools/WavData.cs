using System;
using System.Reflection.PortableExecutable;
using System.Runtime.InteropServices;

namespace AudioTools
{
    public class WavData : AudioData
    {
        public WavData() : this(Console.ReadLine())
        {
            FileType = "Wave";
        }
        public WavData(string filename) : base(filename)
        {
            FileType = "Wave";
            LoadWav();
        }
        public bool LoadWav()
        {
            Left = Right = null;
            //Check header for valid WAV file
            try
            {
                using FileStream fs = File.Open(FileName, FileMode.Open);
                BinaryReader reader = new(fs);
                LoadHeader(reader);

                // Load DATA!
                byte[] byteArray = reader.ReadBytes(HeaderData["bytes"]);

                SampleLength = HeaderData["bitDepth"] / 8;
                int nValues = HeaderData["bytes"] / SampleLength;
                float[] asFloat = AudioFileUtils.ByteArrayToFloat(byteArray, HeaderData["bitDepth"], nValues, HeaderData["bytes"]);
                switch (HeaderData["channels"])
                {
                    case 1:
                        Left = asFloat;
                        Right = null;
                        Samples = Left;
                        return true;
                    case 2:
                        // de-interleave
                        int nSamps = nValues / 2;
                        Left = new float[nSamps];
                        Right = new float[nSamps];
                        for (int s = 0, v = 0; s < nSamps; s++)
                        {
                            Left[s] = asFloat[v++];
                            Right[s] = asFloat[v++];
                        }
                        Samples = asFloat;
                        return true;
                    default:
                        return false;
                }
            }
            catch (FileNotFoundException)
            {
                //Debug.Log("...Failed to load: " + filename);
                Console.WriteLine("Failed to load" + FileName);
                return false;
            }
        }
        public void LoadHeader(BinaryReader reader)
        {
            RawHeader = reader.ReadBytes(44);
            reader.BaseStream.Position = 0;


            // chunk 0
            HeaderData.Add("chunkID", reader.ReadInt32());
            HeaderData.Add("fileSize", reader.ReadInt32());
            HeaderData.Add("riffType", reader.ReadInt32());


            // chunk 1
            HeaderData.Add("fmtID", reader.ReadInt32());
            HeaderData.Add("fmtSize", reader.ReadInt32()); // bytes for this chunk (expect 16 or 18)

            // 16 bytes coming...
            HeaderData.Add("fmtCode", reader.ReadInt16());
            HeaderData.Add("channels", reader.ReadInt16());
            SampleRate = reader.ReadInt32();
            HeaderData.Add("sampleRate", SampleRate);
            HeaderData.Add("byteRate", reader.ReadInt32());
            HeaderData.Add("fmtBlockAlign", reader.ReadInt16());
            HeaderData.Add("bitDepth", reader.ReadInt16());

            if (HeaderData["fmtSize"] == 18)
            {
                // Read any extra values
                HeaderData.Add("fmtExtraSize", reader.ReadInt16());
                reader.ReadBytes(HeaderData["fmtExtraSize"]);
            }

            // chunk 2
            HeaderData.Add("dataID", reader.ReadInt32());
            HeaderData.Add("bytes", reader.ReadInt32());
        }
        public void Format16BitTo32BitWav()
        {
            if (HeaderData["bitDepth"] > 16) { return; }
            //Reformat header file for 32 bit
        }
        public bool PackFloatToWav(string fileNameOut)
        {
            byte[] wavAsBytes = new byte[8 + HeaderData["fileSize"]];
            //Put Header in bytearray first
            Buffer.BlockCopy(RawHeader, 0, wavAsBytes, 0, RawHeader.Length);
            //Switch to handle re-encoding at differnet bitrates
            switch (HeaderData["bitDepth"])
            {
                case 16:
                    //Convert Float(32 bits) back to Int16s
                    short[] shortSamples = new short[Samples.Length];
                    for(int i=0; i < Samples.Length; i++)
                    {
                        shortSamples[i] = (short)Math.Floor(Samples[i] * 32767);
                    }
                    Buffer.BlockCopy(shortSamples, 0, wavAsBytes, RawHeader.Length, HeaderData["bytes"]);
                    Console.WriteLine("CONVETED TIME TO WRITE");
                    //Write byte array to file
                    File.WriteAllBytes(fileNameOut, wavAsBytes);
                    return true;
                case 32:
                    Buffer.BlockCopy(Samples, 0, wavAsBytes, RawHeader.Length, HeaderData["bytes"]);
                    return true;
                case 64:
                    double[] sampleAsDouble = Array.ConvertAll(Samples,e=> (double)e);
                    Buffer.BlockCopy(Samples, 0, wavAsBytes, RawHeader.Length, HeaderData["bytes"]);
                    return true;
            }
            return false;
        }
    }
}

