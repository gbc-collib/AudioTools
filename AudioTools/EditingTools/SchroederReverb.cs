using AudioTools.AudioFileTools;

namespace AudioTools.EditingTools
{
    public class SchroederReverb : ReverbPedal
    {
        public SchroederReverb(IAudioData audioFile, float delay, float decay, float mixPercent) : base(audioFile, delay, decay, mixPercent)
        {
        }
        public override void ApplyEffect()
        {
            if(IsEnabled != true) { return; }
            float originalDelayParam = Delay;
            float originalDecayParam = Decay;
            //4 parallel combfilters, These "magic numbers" being applied to the combFilters come from a scientific paper written on Schroeder's reverb but they can be tweaked for desired sound
            float[] combFilterSamples1 = CombFilter();
            Delay -= 11.73f;
            Decay -= 0.1313f;
            float[] combFilterSamples2 = CombFilter();
            Delay += 19.31f;
            Decay -= 0.2743f;
            float[] combFilterSamples3 = CombFilter();
            Delay -= 7.97f;
            Decay -= 0.31f;
            float[] combFilterSamples4 = CombFilter();
            Delay = originalDelayParam;
            Decay = originalDecayParam;
            float[] outputComb = new float[AudioFile.Samples.Length];
            //Combine into one array
            for (int i = 0; i < AudioFile.Samples.Length; i++)
            {
                outputComb[i] = combFilterSamples1[i] + combFilterSamples2[i] + combFilterSamples3[i] + combFilterSamples4[i];
            }
            //Mix audio wet/dry of effect
            float[] mixAudio = new float[AudioFile.Samples.Length];
            for (int i = 0; i < AudioFile.Samples.Length; i++)
                mixAudio[i] = (100 - MixPercent) * AudioFile.Samples[i] + MixPercent * outputComb[i];
            //Two sequential allpassfilterrs
            AudioFile.Samples = AllPassFilter();
            AudioFile.Samples = AllPassFilter();

        }

        public float[] CombFilter()
        {
            int delaysamples = (int)((float)Delay * (AudioFile.SampleRate / 1000));
            float[] combfiltersamples = new float[AudioFile.SampleLength];
            Array.Copy(AudioFile.Samples, combfiltersamples, AudioFile.SampleLength);
            for (int i = 0; i < AudioFile.SampleLength - Delay; i++)
            {
                combfiltersamples[i + (int)Delay] = combfiltersamples[i + (int)Delay] + combfiltersamples[i] * Decay;
            }

            return combfiltersamples;
        }

        public float[] AllPassFilter()
        {
            int delaySamples = (int)((float)89.27f * (AudioFile.SampleRate / 1000));
            float[] allpassfiltersamples = new float[AudioFile.SampleLength];
            float decayfactor = 0.131f;
            for (int i = 0; i < AudioFile.SampleLength; i++)
            {
                allpassfiltersamples[i] = AudioFile.Samples[i];

                if (i - delaySamples >= 0)
                    allpassfiltersamples[i] += -decayfactor * allpassfiltersamples[i - delaySamples];

                if (i - delaySamples >= 1)
                    allpassfiltersamples[i] += decayfactor * allpassfiltersamples[i + 20 - delaySamples];
            }
            float value = allpassfiltersamples[0];
            float max = 0.0f;
            for (int i = 0; i < AudioFile.SampleLength; i++)
            {
                if (Math.Abs(allpassfiltersamples[i]) > max)
                    max = Math.Abs(allpassfiltersamples[i]);
            }

            for (int i = 0; i < allpassfiltersamples.Length; i++)
            {
                float currentValue = allpassfiltersamples[i];
                value = (value + (currentValue - value)) / max;

                allpassfiltersamples[i] = value;
            }
            return allpassfiltersamples;
        }

    }
}
