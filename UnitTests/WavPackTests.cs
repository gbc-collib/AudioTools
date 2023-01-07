using NUnit.Framework;
using AudioTools;
using System.Security.Cryptography;

[TestFixture]
public class PackWavFileTest
{
    private readonly string outfile1 = "/Users/collinstasisk/Documents/GitHub/AudioTools/AudioTools/PinkPanther30-1.wav";
    private readonly string outfile2 = "/Users/collinstasisk/Documents/GitHub/AudioTools/AudioTools/PinkPanther30-2.wav";
    private readonly string infile1 = "/Users/collinstasisk/Documents/GitHub/AudioTools/AudioTools/PinkPanther30.wav";

    [Test]
    public void SaveFile_WriteTwo16bitFiles_ReturnTrueIfEqual()
    {
        WavData UnalteredSource = new(infile1);
        UnalteredSource.SaveFile(outfile1);
        WavData CopiedFile = new(outfile1);
        Assert.AreEqual(UnalteredSource.Samples, CopiedFile.Samples);
        Assert.AreEqual(UnalteredSource.RawHeader, CopiedFile.RawHeader);
    }
}

///Users/collinstasisk/Documents/GitHub/AudioTools/AudioTools/PinkPanther30.wav

