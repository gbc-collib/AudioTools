using System;
using Microsoft.VisualBasic.FileIO;

namespace AudioTools
{
	public static class WavReader
	{
        public static void LoadHeader(WavData WavObject, BinaryReader reader)
        {
            WavObject.RawHeader = reader.ReadBytes(44);
            reader.BaseStream.Position = 0;


            // chunk 0
            WavObject.HeaderData.Add("chunkID", reader.ReadInt32());
            WavObject.HeaderData.Add("fileSize", reader.ReadInt32());
            WavObject.HeaderData.Add("riffType", reader.ReadInt32());


            // chunk 1
            WavObject.HeaderData.Add("fmtID", reader.ReadInt32());
            WavObject.HeaderData.Add("fmtSize", reader.ReadInt32()); // bytes for this chunk (expect 16 or 18)

            // 16 bytes coming...
            WavObject.HeaderData.Add("fmtCode", reader.ReadInt16());
            WavObject.HeaderData.Add("channels", reader.ReadInt16());
            WavObject.SampleRate = reader.ReadInt32();
            WavObject.HeaderData.Add("sampleRate", WavObject.SampleRate);
            WavObject.HeaderData.Add("byteRate", reader.ReadInt32());
            WavObject.HeaderData.Add("fmtBlockAlign", reader.ReadInt16());
            WavObject.HeaderData.Add("bitDepth", reader.ReadInt16());

            if (WavObject.HeaderData["fmtSize"] == 18)
            {
                // Read any extra values
                WavObject.HeaderData.Add("fmtExtraSize", reader.ReadInt16());
                reader.ReadBytes(WavObject.HeaderData["fmtExtraSize"]);
            }

            // chunk 2
            WavObject.HeaderData.Add("dataID", reader.ReadInt32());
            WavObject.HeaderData.Add("bytes", reader.ReadInt32());
        }
        public static bool LoadWav(WavData WavObject)
        {
            WavObject.Left = WavObject.Right = null;
            //Check header for valid WAV file
            try
            {
                using FileStream fs = File.Open(WavObject.FileName, FileMode.Open);
                BinaryReader binaryReader = new(fs);
                BinaryReader reader = binaryReader;
                LoadHeader(WavObject, reader);

                // Load DATA!
                byte[] byteArray = reader.ReadBytes(WavObject.HeaderData["bytes"]);

                WavObject.SampleLength = WavObject.HeaderData["bitDepth"] / 8;
                int nValues = WavObject.HeaderData["bytes"] / WavObject.SampleLength;
                var asFloat = ByteUtils.ByteArrayToFloat(byteArray, WavObject.HeaderData["bitDepth"], nValues, WavObject.HeaderData["bytes"]);
                switch (WavObject.HeaderData["channels"])
                {
                    case 1:
                        WavObject.Left = WavObject.Samples = asFloat;
                        WavObject.Right = null;
                        return true;
                    case 2:
                        // de-interleave
                        int nSamps = nValues / 2;
                        WavObject.Left = new float[nSamps];
                        WavObject.Right = new float[nSamps];
                        for (int s = 0, v = 0; s < nSamps; s++)
                        {
                            WavObject.Left[s] = asFloat[v++];
                            WavObject.Right[s] = asFloat[v++];
                        }
                        WavObject.Samples = asFloat;
                        return true;
                    default:
                        return false;
                }
            }
            catch (FileNotFoundException)
            {
                //Debug.Log("...Failed to load: " + filename);
                Console.WriteLine("Failed to load" + WavObject.FileName);
                return false;
            }
        }
    }
}

