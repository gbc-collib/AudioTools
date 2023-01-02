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
    public class AudioFileHandler
    {
        //backing fields
        private string _filename = string.Empty;
        //properties
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
        public float[] Left { get; set; }
        public float[] Right { get; set; }

        public AudioFileHandler()
        {
            Console.WriteLine("Type your audio file name to be opened");
            FileName = Console.ReadLine();
            ReadWavToFloat(FileName, out float[] L, out float[] R);
            Left = L;
            Right = R;

        }
        static bool ReadWavToFloat(string filename, out float[] L, out float[] R)
        {
            L = R = null;

            try
            {
                using (FileStream fs = File.Open(filename, FileMode.Open))
                {
                    BinaryReader reader = new(fs);

                    // chunk 0
                    int chunkID = reader.ReadInt32();
                    int fileSize = reader.ReadInt32();
                    int riffType = reader.ReadInt32();


                    // chunk 1
                    int fmtID = reader.ReadInt32();
                    int fmtSize = reader.ReadInt32(); // bytes for this chunk (expect 16 or 18)

                    // 16 bytes coming...
                    int fmtCode = reader.ReadInt16();
                    int channels = reader.ReadInt16();
                    int sampleRate = reader.ReadInt32();
                    int byteRate = reader.ReadInt32();
                    int fmtBlockAlign = reader.ReadInt16();
                    int bitDepth = reader.ReadInt16();

                    if (fmtSize == 18)
                    {
                        // Read any extra values
                        int fmtExtraSize = reader.ReadInt16();
                        reader.ReadBytes(fmtExtraSize);
                    }

                    // chunk 2
                    int dataID = reader.ReadInt32();
                    int bytes = reader.ReadInt32();

                    // DATA!
                    byte[] byteArray = reader.ReadBytes(bytes);

                    int bytesForSamp = bitDepth / 8;
                    int nValues = bytes / bytesForSamp;


                    float[] asFloat = null;
                    switch (bitDepth)
                    {
                        case 64:
                            double[]
                                asDouble = new double[nValues];
                            Buffer.BlockCopy(byteArray, 0, asDouble, 0, bytes);
                            asFloat = Array.ConvertAll(asDouble, e => (float)e);
                            break;
                        case 32:
                            asFloat = new float[nValues];
                            Buffer.BlockCopy(byteArray, 0, asFloat, 0, bytes);
                            break;
                        case 16:
                            Int16[]
                                asInt16 = new Int16[nValues];
                            Buffer.BlockCopy(byteArray, 0, asInt16, 0, bytes);
                            asFloat = Array.ConvertAll(asInt16, e => e / (float)(Int16.MaxValue + 1));
                            break;
                        default:
                            return false;
                    }

                    switch (channels)
                    {
                        case 1:
                            L = asFloat;
                            R = null;
                            return true;
                        case 2:
                            // de-interleave
                            int nSamps = nValues / 2;
                            L = new float[nSamps];
                            R = new float[nSamps];
                            for (int s = 0, v = 0; s < nSamps; s++)
                            {
                                L[s] = asFloat[v++];
                                R[s] = asFloat[v++];
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
                Console.WriteLine("Failed to load" + filename);
                return false;
            }
        }
        static bool FloatToWav(string filename, out float[] audiodata)
        {
            
        }
    }
}

