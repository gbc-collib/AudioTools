using System;
using System.Linq;
using System.Text;
namespace AudioFileHandler.UnitTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AudioTools;

[TestClass]
public class PackWavFileTest
{
    private readonly string outfile1 = "/Users/collinstasisk/Documents/GitHub/AudioTools/AudioTools/PinkPanther30-1.wav";
    private readonly string outfile2 = "/Users/collinstasisk/Documents/GitHub/AudioTools/AudioTools/PinkPanther30-2.wav";
    private readonly string infile1 = "/Users/collinstasisk/Documents/GitHub/AudioTools/AudioTools/PinkPanther30.wav";
    public PackWavFileTest() { }

    [TestMethod]
    public void PackFloatToWav_WriteTwoFiles_VerifyEquality()
    {
        WavData UnalteredSource = new(infile1);
        UnalteredSource.PackFloatToWav(outfile1);
        WavData CopiedFile = new(outfile1);
        if (CopiedFile.Samples.SequenceEqual(UnalteredSource.Samples))
        {
            Assert.IsFalse(false);
        }
        else
        {
            Assert.IsTrue(false);
        }
    }
    [TestMethod]
    public void PackFloatToWav_WriteTwoFiles_VerifyHeaderDataIntegrity()
    {
        WavData UnalteredSource = new(infile1);
        UnalteredSource.PackFloatToWav(outfile1);
        WavData CopiedFile = new(outfile1);
        if (CopiedFile.RawHeader.SequenceEqual(UnalteredSource.RawHeader))
        {
            Console.WriteLine("Pack To Float sucessfully copied Header Data Unaltered");
        }
        else
        {
            Assert.IsTrue(false);
        }
    }
}

///Users/collinstasisk/Documents/GitHub/AudioTools/AudioTools/PinkPanther30.wav

