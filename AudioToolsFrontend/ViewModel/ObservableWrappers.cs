using AudioTools.AudioFileTools;
using AudioTools.EditingTools;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace AudioToolsFrontend.ViewModel
{
    [ObservableObject]
    public partial class ObservableSchroederReverb : SchroederReverb
    {

        public Dictionary<string, float> ObservableParameters
        {
            get => Parameters;
            set
            {
                Debug.WriteLine($"Chaning to {value}");
                SetProperty(Parameters, value, this, (u, n) => u.Parameters = n);
            }
        }
        public ObservableSchroederReverb(IAudioData audioFile, float delay, float decay, float mixPercent) : base(audioFile, delay, decay, mixPercent)
        {
            {
                Parameters = new Dictionary<string, float>()
                {
                    { "Delay", delay },
                    { "Decay", decay },
                    { "MixPercent", mixPercent }
                };
                Delay = delay;
                Decay = decay;
                MixPercent = mixPercent;
                AudioFile = audioFile;
                IsEnabled = false;
            }
        }
    }

    [ObservableObject]
    public partial class ObservableOverDrive : OverDriveDistortion
    {

        public Dictionary<string, float> ObservableParameters
        {
            get => Parameters;
            set
            {
                Debug.WriteLine($"Chaning to {value}");
                SetProperty(Parameters, value, this, (u, n) => u.Parameters = n);
            }
        }
        public ObservableOverDrive(IAudioData audioFile, float gain, float lowPassCutoff, float highPassCutOff) : base(audioFile, gain, lowPassCutoff, highPassCutOff)
        {
            {
                Parameters = new Dictionary<string, float>()
                {
                    { "Delay", gain },
                    { "Decay", lowPassCutoff },
                    { "MixPercent", highPassCutOff }
                };
                Gain = gain;
                LowPassCutOff = lowPassCutoff;
                HighPassCutOff = highPassCutOff;
                AudioFile = audioFile;
                IsEnabled = false;
            }
        }
    }
}