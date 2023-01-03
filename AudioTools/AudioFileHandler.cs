using System;
using System.Diagnostics;
using System.Text;
//Test Audio file at
///Users/collinstasisk/Documents/GitHub/AudioTools/AudioTools/PinkPanther30.wav
namespace AudioTools
{
    public static class ByteUtils
    {
        static short LittleEndianBytesToShort(byte[] data, int index)
        {
            return (short)((data[index + 1] << 8) | data[index]);
        }
        static byte[] FloattoLittleEndianBytes(short data)
        {
            byte[] bytes = new byte[2]; //A short is 16 bits so it can always be represented in 2 bytes
            bytes[0] = (byte)data;
            bytes[1] = (byte)(data >> 8 & 0xFF);
            return bytes;
        }
    }
    public static class AudioFileHandler
    {
        public static bool ReadWavToFloat(AudioData audioFile)
        {
            audioFile.Left = audioFile.Right = null;

            try
            {
                using (FileStream fs = File.Open(audioFile.FileName, FileMode.Open))
                {
                    BinaryReader reader = new(fs);

                    // chunk 0
                    audioFile.HeaderData.Add("chunkID", reader.ReadInt32());
                    audioFile.HeaderData.Add("fileSize", reader.ReadInt32());
                    audioFile.HeaderData.Add("riffType", reader.ReadInt32());


                    // chunk 1
                    audioFile.HeaderData.Add("fmtID", reader.ReadInt32());
                    audioFile.HeaderData.Add("fmtSize", reader.ReadInt32()); // bytes for this chunk (expect 16 or 18)

                    // 16 bytes coming...
                    audioFile.HeaderData.Add("fmtCode", reader.ReadInt16());
                    audioFile.HeaderData.Add("channels", reader.ReadInt16());
                    audioFile.SampleRate = reader.ReadInt32();
                    audioFile.HeaderData.Add("byteRate", reader.ReadInt32());
                    audioFile.HeaderData.Add("fmtBlockAlign", reader.ReadInt16());
                    audioFile.HeaderData.Add("bitDepth", reader.ReadInt16());

                    if (audioFile.HeaderData["fmtSize"] == 18)
                    {
                        // Read any extra values
                        audioFile.HeaderData.Add("fmtExtraSize", reader.ReadInt16());
                        reader.ReadBytes(audioFile.HeaderData["fmtExtraSize"]);
                    }

                    // chunk 2
                    audioFile.HeaderData.Add("dataID", reader.ReadInt32());
                    audioFile.HeaderData.Add("bytes", reader.ReadInt32());

                    // DATA!
                    byte[] byteArray = reader.ReadBytes(audioFile.HeaderData["bytes"]);

                     audioFile.SampleLength = audioFile.HeaderData["bitDepth"] / 8;
                    audioFile.HeaderData["sampleLength"] = audioFile.SampleLength;
                    int nValues = audioFile.HeaderData["bytes"] / audioFile.HeaderData["sampleLength"];


                    float[] asFloat = null;
                    switch (audioFile.HeaderData["bitDepth"])
                    {
                        case 64:
                            double[]
                                asDouble = new double[nValues];
                            Buffer.BlockCopy(byteArray, 0, asDouble, 0, audioFile.HeaderData["bytes"]);
                            asFloat = Array.ConvertAll(asDouble, e => (float)e);
                            break;
                        case 32:
                            asFloat = new float[nValues];
                            Buffer.BlockCopy(byteArray, 0, asFloat, 0, audioFile.HeaderData["bytes"]);
                            break;
                        case 16:
                            Int16[]
                                asInt16 = new Int16[nValues];
                            Buffer.BlockCopy(byteArray, 0, asInt16, 0, audioFile.HeaderData["bytes"]);
                            asFloat = Array.ConvertAll(asInt16, e => e / (float)(Int16.MaxValue + 1));
                            break;
                        default:
                            return false;
                    }

                    switch (audioFile.HeaderData["channels"])
                    {
                        case 1:
                            audioFile.Left = asFloat;
                            audioFile.Right = null;
                            return true;
                        case 2:
                            // de-interleave
                            int nSamps = nValues / 2;
                            audioFile.Left = new float[nSamps];
                            audioFile.Right = new float[nSamps];
                            for (int s = 0, v = 0; s < nSamps; s++)
                            {
                                audioFile.Left[s] = asFloat[v++];
                                audioFile.Right[s] = asFloat[v++];
                            }
                            return true;
                        default:
                            return false;
                    }
                }
            }
            catch (FileNotFoundException)
            {
                //Debug.Log("...Failed to load: " + filename);
                Console.WriteLine("Failed to load" + audioFile.FileName);
                return false;
            }
        }
        public static bool PackFloatToWav(string fileNameOut, AudioData audioData)
        {
            byte[] wavAsBytes = new byte[8 + audioData.HeaderData["fileSize"]];
            //Write Header Meta-Data
            byte[] header = CreateWavHeaderFile(audioData);
            wavAsBytes.Concat(header).ToArray();
            Console.WriteLine(audioData.Samples.Length);
            Console.WriteLine(audioData.Samples[0]);
           //byte[] data = new byte[audioData.HeaderData["bytes"]];
            Buffer.BlockCopy(audioData.Samples, 0, wavAsBytes, header.Length, audioData.SampleLength * 8);       
            Console.WriteLine("CONVETED TIME TO WRITE");
            File.WriteAllBytes("/Users/collinstasisk/Documents/GitHub/AudioTools/AudioTools/PinkPanther302.wav", wavAsBytes);
            return true;
        }
        static byte[] CreateWavHeaderFile(AudioData audioData)
        {
            byte[] header = new byte[44];
            /*
            header.Concat(BitConverter.GetBytes(audioData.HeaderData["chunkID"])).ToArray();
            header.Concat(BitConverter.GetBytes(audioData.HeaderData["fileSize"])).ToArray();
            header.Concat(BitConverter.GetBytes(audioData.HeaderData["riffType"])).ToArray();
            */
            foreach(var item in audioData.HeaderData)
            {
                header.Concat(BitConverter.GetBytes(item.Value)).ToArray();
            }
            return header;
        }
    }
}

