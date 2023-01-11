using AudioTools.AudioFileTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/* The IPedal is part of the Adapter Design Pattern to improve maintence of the code and create easier use cases for the gui */
namespace AudioTools.EditingTools
{
    public interface IPedal
    {
        IAudioData AudioFile { get; set; }
        public Dictionary<string, float> Parameters { get; set; }
        public bool IsEnabled { get; set; }
        void ApplyEffect();
    }
}
