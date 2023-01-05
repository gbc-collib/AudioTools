using System;
using System.Linq;
using System.Text;
namespace AudioFileHandler.UnitTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AudioTools;
using System.Runtime.Intrinsics.X86;

[TestClass]
public class ByteConversionTest
{
    [TestMethod]
    public void ByteArrayToFloat_Given32bitMockArray_VerifyProperConversion()
    {
        byte[] mockArray = BitConverter.GetBytes(234.2f);
        float[] testFloat = ByteUtils.ByteArrayToFloat(mockArray, 32, 1, mockArray.Length);
        if (testFloat[0] == BitConverter.ToSingle(mockArray))
            Assert.IsTrue(true);
        else
        {
            Assert.IsTrue(false);
        }
    }
    [TestMethod]
    public void ByteArrayToFloat_Given16bitMockArray_VerifyProperConversion()
    {
        byte[] mockArray = BitConverter.GetBytes((short)15);
        float[] testFloat = ByteUtils.ByteArrayToFloat(mockArray, 16, 1, mockArray.Length);
        if (testFloat[0] == BitConverter.ToSingle(mockArray))
            Assert.IsTrue(true);
        else
        {
            Assert.IsTrue(false);
        }
    }
    [TestMethod]
    public void ByteArrayToFloat_Given64bitMockArray_VerifyProperConversion()
    {
        double mockDouble = 15.123;
        byte[] mockArray = BitConverter.GetBytes(mockDouble);
        float[] testFloat = ByteUtils.ByteArrayToFloat(mockArray, 64, 1, mockArray.Length);
        if (testFloat[0] == Convert.ToSingle(mockDouble))
            Assert.IsTrue(true);
        else
        {
            Assert.IsTrue(false);
        }
    }
}