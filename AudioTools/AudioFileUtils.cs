using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using static System.Net.Mime.MediaTypeNames;
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
    public static class AudioFileUtils
    {
        public static float[] ByteArrayToFloat(byte[] byteArray, int inputBitRate, int floatLen, int dataLen)
        {
            float[] asFloat = new float[floatLen];
            switch (inputBitRate)
            {
                case 64:
                    double[]
                        asDouble = new double[floatLen];
                    Buffer.BlockCopy(byteArray, 0, asDouble, 0, dataLen);
                    asFloat = Array.ConvertAll(asDouble, e => (float)e);
                    break;
                case 32:
                    Buffer.BlockCopy(byteArray, 0, asFloat, 0, dataLen);
                    break;
                case 16:
                    Int16[] asInt16 = new Int16[floatLen];
                    Buffer.BlockCopy(byteArray, 0, asInt16, 0, dataLen);
                    asFloat = Array.ConvertAll(asInt16, e => e / (float)(Int16.MaxValue + 1));
                    break;
            }
            return asFloat;
        }
        public static byte[] ConvertTo32Bit(float[] samples, int startingByteRate, int dataLen)
        {
            byte[] Byte32Array = new byte[dataLen];
            return Byte32Array;
        }
    }
}

