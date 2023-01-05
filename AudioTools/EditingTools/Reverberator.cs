using System;
namespace AudioTools
{
    public static class Reverberator
    {

        public static void SchroederReverb(IAudioData audioFile, float delay, float decay, int mixPercent)
        {
            //4 parallel combfilters
            float[] combFilterSamples1 = CombFilter(audioFile.Samples, audioFile.Samples.Length, delay, decay, audioFile.SampleRate);
            float[] combFilterSamples2 = CombFilter(audioFile.Samples, audioFile.Samples.Length, (delay - 11.73f), (decay - 0.1313f), audioFile.SampleRate);
            float[] combFilterSamples3 = CombFilter(audioFile.Samples, audioFile.Samples.Length, (delay + 19.31f), (decay - 0.2743f), audioFile.SampleRate);
            float[] combFilterSamples4 = CombFilter(audioFile.Samples, audioFile.Samples.Length, (delay - 7.97f), (decay - 0.31f), audioFile.SampleRate);
            float[] outputComb = new float[audioFile.Samples.Length];
            //Combine into one array
            for (int i = 0; i < audioFile.Samples.Length; i++)
            {
                outputComb[i] = ((combFilterSamples1[i] + combFilterSamples2[i] + combFilterSamples3[i] + combFilterSamples4[i]));
            }
            //Mix audio wet/dry of effect
            float[] mixAudio = new float[audioFile.Samples.Length];
            for (int i = 0; i < audioFile.Samples.Length; i++)
                mixAudio[i] = ((100 - mixPercent) * audioFile.Samples[i]) + (mixPercent * outputComb[i]);
            //Two sequential allpassfilterrs
            float[] allPassFilterSamples1 = AllPassFilter(mixAudio, audioFile.Samples.Length, audioFile.SampleRate);
            float[] allPassFilterSamples2 = AllPassFilter(allPassFilterSamples1, audioFile.Samples.Length, audioFile.SampleRate);
            //To to put back into bytes then into wave
            //Pass reverbed sample back to AudioData class, allow class to handle saving file
            audioFile.Samples = allPassFilterSamples2;

        }

        public static float[] CombFilter(float[] samples, int samplelength, float delay, float decay, float samplerate)
        {
            int delaysamples = (int)((float)delay * (samplerate / 1000));
            float[] combfiltersamples = new float[samplelength];
            Array.Copy(samples, combfiltersamples, samplelength);
            for (int i = 0; i<samplelength - delay; i++)
            {
                combfiltersamples[i + (int)delay] = combfiltersamples[i + (int)delay] + (combfiltersamples[i] * decay);
            }

            return combfiltersamples;
        }

        public static float[] AllPassFilter(float[] samples, int sampleslength, float samplerate) {
            int delaySamples = (int)((float)89.27f * (samplerate / 1000));
            float[] allpassfiltersamples = new float[sampleslength];
            float decayfactor = 0.131f;
            for (int i = 0; i < sampleslength; i++)
            {
                allpassfiltersamples[i] = samples[i];

                if (i - delaySamples >= 0)
                    allpassfiltersamples[i] += -decayfactor * allpassfiltersamples[i - delaySamples];

                if (i - delaySamples >= 1)
                    allpassfiltersamples[i] += decayfactor * allpassfiltersamples[i + 20 - delaySamples];
            }
            float value = allpassfiltersamples[0];
            float max = 0.0f;
            for (int i = 0; i < sampleslength; i++)
            {
                if (Math.Abs(allpassfiltersamples[i]) > max)
                    max = Math.Abs(allpassfiltersamples[i]);
            }

            for (int i = 0; i < allpassfiltersamples.Length; i++)
            {
                float currentValue = allpassfiltersamples[i];
                value = ((value + (currentValue - value)) / max);

                allpassfiltersamples[i] = value;
            }
            return allpassfiltersamples;
        }

    }
}
