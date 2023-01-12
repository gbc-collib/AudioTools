using AudioTools.AudioFileTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioTools.EditingTools
{
    public abstract class DistortionPedal : IPedal
    {
        public IAudioData AudioFile { get; set; }
        public float HighPassCutOff
        {
            get { return Parameters["HighPassCutOff"]; }
            set { Parameters["HighPassCutOff"] = value; }
        }
        public float LowPassCutOff
        {
            get { return Parameters["LowPassCutOff"]; }
            set { Parameters["LowPassCutOff"] = value; }
        }
        public float Gain
        {
            get { return Parameters["Gain"]; }
            set { Parameters["Gain"] = value; }
        }
        public bool IsEnabled { get; set; } = true;
        public Dictionary<string, float> Parameters { get; set; }

        public abstract void ApplyEffect();

        public DistortionPedal(IAudioData audioFile, float gain, float lowPassCutOff, float highPassCutOff)
        {
            Parameters = new Dictionary<string, float>() {
            { "Gain", gain },
            { "LowPassCutOff", lowPassCutOff },
            { "HighPassCutOff", highPassCutOff }
        };
            AudioFile = audioFile;
            Gain = gain;
            LowPassCutOff = lowPassCutOff;
            HighPassCutOff = highPassCutOff;
        }
    }
}
