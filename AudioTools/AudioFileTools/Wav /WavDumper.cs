using System;
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
    }
}

