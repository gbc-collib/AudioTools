using AudioTools.AudioFileTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioTools.EditingTools
{
    public abstract class ReverbPedal : IPedal
    {
        public float Delay
        {
            get { return Parameters["Delay"]; }
            set { Parameters["Delay"] = value; }
        }
        public float Decay
        {
            get { return Parameters["Decay"]; }
            set { Parameters["Decay"] = value; }
        }
        public float MixPercent
        {
            get { return Parameters["MixPercent"]; }
            set { Parameters["MixPercent"] = value; }
        }
        public bool IsEnabled { get; set; } = true;
        public IAudioData AudioFile { get; set; }
        public Dictionary<string, float> Parameters { get; set; }
        public abstract void ApplyEffect();
        public ReverbPedal(IAudioData audioFile, float delay, float decay, float mixPercent)
        {
            Delay = delay;
            Decay = decay;
            MixPercent = mixPercent;
            AudioFile = audioFile;
            Parameters = new Dictionary<string, float>() {
            { "Delay", delay },
            { "Decay", decay },
            { "MixPercent", mixPercent }
        };
        }
    }
}
