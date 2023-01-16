namespace AudioTools.AudioFileTools.Wav
{
    public static class WavDumper
    {
        public static bool PackSamplesToWav(WavData wavObject, string fileNameOut)
        {
            if(SampleArrayToBytes(wavObject, out byte[] wavAsBytes) != true)
            { return false; }
            File.WriteAllBytes(fileNameOut, wavAsBytes);
            return true;

        }
        public static bool SampleArrayToBytes(WavData wavObject, out byte[] wavAsBytes)
        {
            wavAsBytes = new byte[8 + wavObject.HeaderData["fileSize"]];
            //Put Header in bytearray first
            Buffer.BlockCopy(wavObject.RawHeader, 0, wavAsBytes, 0, wavObject.RawHeader.Length);
            //Switch to handle re-encoding at differnet bitrates
            switch (wavObject.HeaderData["bitDepth"]) //Convert File to original bit size and return true if implementation for that bit-rate exists
            {
                case 16:
                    {
                        //Convert Float(32 bits) back to Int16s
                        short[] shortSamples = new short[wavObject.Samples.Length];
                        for (int i = 0; i < wavObject.Samples.Length; i++)
                        {
                            shortSamples[i] = (short)Math.Floor(wavObject.Samples[i] * 32767);
                        }
                        Buffer.BlockCopy(shortSamples, 0, wavAsBytes, wavObject.RawHeader.Length, wavObject.HeaderData["bytes"]);
                        return true;
                    }
                case 32:
                    {
                        Buffer.BlockCopy(wavObject.Samples, 0, wavAsBytes, wavObject.RawHeader.Length, wavObject.HeaderData["bytes"]);
                        return true;
                    }
                case 64:
                    {
                        double[] sampleAsDouble = Array.ConvertAll(wavObject.Samples, e => (double)e);
                        Buffer.BlockCopy(wavObject.Samples, 0, wavAsBytes, wavObject.RawHeader.Length, wavObject.HeaderData["bytes"]);
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

