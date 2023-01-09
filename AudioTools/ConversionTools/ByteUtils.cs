//Test Audio file at
///Users/collinstasisk/Documents/GitHub/AudioTools/AudioTools/PinkPanther30.wav
namespace AudioTools.ConversionTools
{
    public static class ByteUtils
    {
        //Converts an array of Little Endian Bytes to a short int value
        static short LittleEndianBytesToShort(byte[] data, int index)
        {
            return (short)(data[index + 1] << 8 | data[index]);
        }
        //Converts short to Little endian byte 
        static byte[] ShorttoLittleEndianBytes(short data)
        {
            byte[] bytes = new byte[2]; //A short is 16 bits so it can always be represented in 2 bytes
            bytes[0] = (byte)data;
            bytes[1] = (byte)(data >> 8 & 0xFF);
            return bytes;
        }
        //All of these cases will return a float data type representing byte array
        //We use float to add precision to calculations as testing using integers proved that
        //the rounding causes distortion in the final product
        //Support for 16, 32, 64 bit .wavs is currently implemented
        public static float[] ByteArrayToFloat(byte[] byteArray, int inputBitRate, int floatLen, int dataLen)
        {
            float[] asFloat = new float[floatLen];
            switch (inputBitRate)
            {
                case 64:
                    {
                        double[]
                                            asDouble = new double[floatLen];
                        Buffer.BlockCopy(byteArray, 0, asDouble, 0, dataLen);
                        asFloat = Array.ConvertAll(asDouble, e => (float)e);
                        break;
                    }
                case 32:
                    {
                        Buffer.BlockCopy(byteArray, 0, asFloat, 0, dataLen);
                        break;
                    }
                case 16:
                    {
                        short[] asInt16 = new short[floatLen];
                        //Block Copy handles our conversion from bytes to in16
                        Buffer.BlockCopy(byteArray, 0, asInt16, 0, dataLen);
                        asFloat = Array.ConvertAll(asInt16, e => e / (float)(short.MaxValue + 1));
                        break;
                    }

                default:
                    throw new Exception("Unexpected Case try using a wav of 16, 32, 64 bit rate");
            }
            return asFloat;
        }
    }
}

