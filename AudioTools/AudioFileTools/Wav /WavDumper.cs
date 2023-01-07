using System;
using System.Text;

namespace AudioTools
{
	public static class WavDumper
	{
        public static bool PackSamplesToWav(WavData WavObject, string fileNameOut)
        {
            byte[] wavAsBytes = new byte[8 + WavObject.HeaderData["fileSize"]];
            //Put Header in bytearray first
            Buffer.BlockCopy(WavObject.RawHeader, 0, wavAsBytes, 0, WavObject.RawHeader.Length);
            //Switch to handle re-encoding at differnet bitrates
            switch (WavObject.HeaderData["bitDepth"])
            {
                case 16:
                    {
                        //Convert Float(32 bits) back to Int16s
                        short[] shortSamples = new short[WavObject.Samples.Length];
                        for (int i = 0; i < WavObject.Samples.Length; i++)
                        {
                            shortSamples[i] = (short)Math.Floor(WavObject.Samples[i] * 32767);
                        }
                        Buffer.BlockCopy(shortSamples, 0, wavAsBytes, WavObject.RawHeader.Length, WavObject.HeaderData["bytes"]);
                        Console.WriteLine("CONVETED TIME TO WRITE");
                        //Write byte array to file
                        File.WriteAllBytes(fileNameOut, wavAsBytes);
                        return true;
                    }
                case 32:
                    {
                        Buffer.BlockCopy(WavObject.Samples, 0, wavAsBytes, WavObject.RawHeader.Length, WavObject.HeaderData["bytes"]);
                        return true;
                    }
                case 64:
                    {
                        double[] sampleAsDouble = Array.ConvertAll(WavObject.Samples, e => (double)e);
                        Buffer.BlockCopy(WavObject.Samples, 0, wavAsBytes, WavObject.RawHeader.Length, WavObject.HeaderData["bytes"]);
                        return true;
                    }

                default:
                    {
                        return false;
                    }
            }
        }
        public static byte[] WriteWavWithCustomerHeader(byte[] data, int dataSize, int numChannels, int sampleRate, int bytesPerSample, int bitsPerSample)
        {
            // Create a MemoryStream to hold the WAV file data
            using (MemoryStream ms = new MemoryStream())
            {
                // Write the WAV file header
                ms.Write(Encoding.ASCII.GetBytes("RIFF"), 0, 4);
                ms.Write(BitConverter.GetBytes(dataSize + 36), 0, 4);
                ms.Write(Encoding.ASCII.GetBytes("WAVE"), 0, 4);
                ms.Write(Encoding.ASCII.GetBytes("fmt "), 0, 4);
                ms.Write(BitConverter.GetBytes(16), 0, 4);
                ms.Write(BitConverter.GetBytes((short)1), 0, 2);
                ms.Write(BitConverter.GetBytes((short)numChannels), 0, 2);
                ms.Write(BitConverter.GetBytes(sampleRate), 0, 4);
                ms.Write(BitConverter.GetBytes(sampleRate * numChannels * bytesPerSample), 0, 4);
                ms.Write(BitConverter.GetBytes((short)(numChannels * bytesPerSample)), 0, 2);
                ms.Write(BitConverter.GetBytes((short)bitsPerSample), 0, 2);
                ms.Write(Encoding.ASCII.GetBytes("data"), 0, 4);
                ms.Write(BitConverter.GetBytes(dataSize), 0, 4);
                ms.Write(data, 0, dataSize);
                byte[] bytes = ms.ToArray();
                ms.Dispose();
                return bytes;
            }
        }
    }
}

