using AudioTools.AudioFileTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioTools.AudioFileTools
{
    static public class FileHandler
    {
        //Method to figure out file type and pass it to correct audio file handling class
        public static IAudioData HandleFileTypes(string filePath, string fileType = null)
        {
            if (string.IsNullOrWhiteSpace(fileType))
            {
                var file = new FileInfo(filePath);
                fileType = file.Extension;
            }

            switch (fileType)
            {
                case "Wave":
                    {
                        Wav.WavData fileData = new(filePath);
                        return fileData;
                    }
                default:
                    {
                        throw new NotImplementedException();
                    }
            }
        }
    }
}