using System;
using AudioTools.AudioFileTools;

/*
 * Start by generating a modulating signal, which is often a sine wave that is 
 * slightly detuned from the original signal. The frequency of the modulating 
 * signal is controlled by the rate parameter, and the amount of detuning is 
 * controlled by the depth parameter.
Mix the modulating signal with the dry signal to create the modulated signal.
This is done by multiplying the dry signal with the modulating signal at each sample.
Apply a low-pass filter to the modulated signal to remove any high-frequency
content that was added during the modulation process. This helps to reduce the
harshness of the effect and make it sound more natural.
Use an envelope generator to control the amount of modulation applied to the
signal over time. The envelope generator is used to create a smooth transition
between the modulated and dry signal, this can be done by using an envelope
generator to modulate the gain of the modulated signal.
Mix the dry signal and the modulated signal together to create the final chorus
effect. The mix parameter controls the balance between the dry and modulated signals.
Add the final output to the output buffer.
The exact implementation of the algorithm will depend on the programming */

namespace AudioTools.EditingTools
{
    public class ChorusPedal : IPedal
    {
        public ChorusPedal(IAudioData audioData, float mixPercent, float depth, float rate)
        {
            AudioFile = audioData;
            Parameters = new Dictionary<string, float>
            {
                {"MixPercent",  mixPercent },
                {"Depth", depth },
                {"Rate", rate }
            };
            MixPercent = mixPercent;
            Depth = depth;
            Rate = rate;
        }
        public IAudioData AudioFile { get; set; }
        public Dictionary<string, float> Parameters { get; set; }
        public bool IsEnabled { get; set; }
        public float MixPercent
        {
            get { return Parameters["MixPercent"]; }
            set { Parameters["MixPercent"] = value; }
        }
        public float Depth
        {
            get { return Parameters["Depth"]; }
            set { Parameters["Depth"] = value; }
        }
        public float Rate
        {
            get { return Parameters["Rate"]; }
            set { Parameters["Rate"] = value; }
        }

        //Combine the dry and wet signal at specified rate by Mixpercent
        public void ApplyEffect()
        {
            var output = new float[AudioFile.Samples.Length];
            output = ModulateSignal(AudioFile.Samples);
            output = CombineWetDry(output);

        }
        public float[] CombineWetDry(float[] wetInput)
        {
            var mixAudio = new float[AudioFile.Samples.Length];
            for (int i = 0; i < AudioFile.Samples.Length; i++)
                mixAudio[i] = (100 - MixPercent) * AudioFile.Samples[i] + MixPercent * wetInput[i];
            return mixAudio;
        }
        /* we're gonna use this formula for sin wave modulation y(t) = A * sin(2 * pi * f * t + phi)
         * Where y(t) is an element of the audio sample array
         * A is the amplitude of our wave which in this instance will represent the 'detune' being applied to the signal
         * f is the frequency, To calculate the frequency value you need to use this formula: f = 1 / (sample rate / desired frequency(we call it rate)
         * T is the time measured in seconds so we can calculate this pretty easily by, t = sample index / sample rate
         * phi is not used in this instance so we'll just set it to 0
         * */
        
        public float[] ModulateSignal(float[] input)
        {
            var output = new float[AudioFile.Samples.Length];
            float amplitude = float.MaxValue;
            float frequency = 1 / (AudioFile.SampleRate / Rate);
            for(int i=0; i< AudioFile.Samples.Length; i++)
            {
                output[i] = (float)(amplitude * Math.Sin(2 * Math.PI * frequency * (i / AudioFile.SampleRate) + 0 ));
            }
            return output;
        }
    }
}

