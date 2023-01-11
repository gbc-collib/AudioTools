using AudioTools.AudioFileTools;

namespace AudioTools.EditingTools
{
    /*
     * Use a low-pass filter to remove high-frequency content from the distorted signal. 
     * This will help to shape the character of the distortion and reduce harshness.
Use a wave shaping function to further distort the signal. This can be implemented using a variety of techniques, such as polynomial functions,
    trigonometric functions, or lookup tables.
Use a high-pass filter to remove low-frequency content from the distorted signal. This can help to
    eliminate unwanted rumble or distortion of the bass frequencies.
Use an equalization (EQ) curve to fine-tune the frequency response of the distorted signal. This can be used to
    boost or cut specific frequency ranges to create the desired tonal character.
    */
    public class OverDriveDistortion : DistortionPedal
    {
        public OverDriveDistortion(IAudioData audioFile, float gain, float lowPassCutOff, float highPassCutOff) : base(audioFile, gain, lowPassCutOff, highPassCutOff)
        {
        }

        public override void ApplyEffect()
        {
            if (IsEnabled != true) { return; }
            AudioFile.Samples = AmplifySignal();
            AudioFile.Samples = ButtersworthLowPassFilter(3);
            AudioFile.Samples = SoftClipShaper();
            AudioFile.Samples = ButtersworthHighPassFilter(3);
            //output = ApplyNoiseGate(output, 400, 600, audioFile.SampleRate);
        }
        public float[] AmplifySignal()
        {
            float[] output = new float[AudioFile.Samples.Length];

            for (int i = 0; i < AudioFile.Samples.Length; i++)
            {
                output[i] = AudioFile.Samples[i] * Gain;
            }
            return output;
        }
        public float[] ApplyNoiseGate(float threshold, float timeConstant)
        {
            int length = AudioFile.Samples.Length;
            float[] output = new float[length];

            float envelope = 0.0f;
            float attack = (1.0f - envelope) / (timeConstant * AudioFile.SampleRate);
            float release = envelope / (timeConstant * AudioFile.SampleRate);

            for (int i = 0; i < length; i++)
            {
                float inputSample = Math.Abs(AudioFile.Samples[i]);
                if (inputSample > envelope)
                {
                    envelope = attack * (inputSample - envelope) + envelope;
                }
                else
                {
                    envelope = release * (inputSample - envelope) + envelope;
                }

                if (envelope > threshold)
                {
                    output[i] = AudioFile.Samples[i];
                }
                else
                {
                    output[i] = 0.0f;
                }
            }

            return output;
        }
        //The buttersworth function is used to roll high end off audio
        //The order parameter referrs to how many 'poles' the function will have
        //Higher order filters will be more computationally intense but they will have steeper roll off
        //Differential equation for buttersworth filter is
        //y[n] = b[0]*x[n] + b[1]*x[n-1] + ... + b[M]*x[n-M] - a[1]*y[n-1] - ... - a[N]*y[n-N]
        public float[] ButtersworthLowPassFilter(int order)
        {
            float[] output = new float[AudioFile.Samples.Length];
            float[] coeffcientA = new float[order + 1];
            float[] coeffcientB = new float[order + 1];
            ComputeButtersWorthLowFilterCoeffcients(order, ref coeffcientA, ref coeffcientB);
            for (int n = 0; n < AudioFile.Samples.Length; n++)
            {
                float y = 0;
                for (int i = 0; i <= order; i++)
                {
                    if (n - i >= 0)
                        y += coeffcientB[i] * AudioFile.Samples[n - i] - coeffcientA[i] * output[n - i];
                }
                output[n] = y;
            }
            return output;
        }
        public void ComputeButtersWorthLowFilterCoeffcients(int order,
            ref float[] coeffcientA, ref float[] coeffcientB)
        {
            for (int i = 0; i < order; i++)
            {
                //This is another part of Butterworth equation that normalizes audio roll the audio off
                //The volume of the audio will get lower the closer we get to the cutoff
                float normilizationFactor = (float)Math.Pow((2 * LowPassCutOff / AudioFile.SampleRate), i);
                coeffcientA[i] = normilizationFactor / Factorial(i);
                coeffcientB[i] = normilizationFactor / Factorial(i + 1);
            }
        }
        //Simple function to calculate factorials because it is used in low-pass equation 
        public static int Factorial(int input)
        {
            int output = 1;
            for (int i = 1; i <= input; i++)
            {
                output = output * i;
            }
            return output;
        }
        public float[] SoftClipShaper()
        {
            float[] output = new float[AudioFile.Samples.Length];
            for (int i = 0; i < AudioFile.Samples.Length; i++)
            {
                //output[i] = 1.5f * (input[i]) - 0.5f * (float)Math.Pow(input[i], 3);
                output[i] = AudioFile.Samples[i] / (1 + Math.Abs(AudioFile.Samples[i]));
            }
            return output;
        }
        public float[] ButtersworthHighPassFilter(int order)
        {
            float[] output = new float[AudioFile.Samples.Length];
            float[] coeffcientB = new float[order + 1];
            float[] coeffcientA = new float[order + 1];
            ComputeButtersWorthHighFilterCoeffcients(order, ref coeffcientB, ref coeffcientA);
            for (int n = 0; n < AudioFile.Samples.Length; n++)
            {
                float y = 0;
                for (int i = 0; i <= order; i++)
                {
                    if (n - i >= 0)
                        y += coeffcientB[i] * AudioFile.Samples[n - i] - coeffcientA[i] * output[n - i];
                }
                output[n] = y;
            }
            return output;
        }
        public void ComputeButtersWorthHighFilterCoeffcients(int order,
            ref float[] coeffcientA, ref float[] coeffcientB)
        {
            {
                //Q in the transfer formula represents how fast the volume drops as we approach the cutoff
                float Q = (float)(Math.Sqrt(Math.Pow(2, 1.0 / order) - 1) / (Math.Pow(2, 1.0 / order) - 1));
                for (int i = 0; i <= order; i++)
                {
                    // This is another part of Butterworth equation that normalizes the audio roll-off
                    // The volume of the audio will get lower the closer we get to the cutoff
                    float normilizationFactor = (float)Math.Pow((2 * HighPassCutOff / AudioFile.SampleRate), i);
                    coeffcientB[i] = normilizationFactor;
                    if (i == 0)
                        coeffcientA[i] = 1;
                    else if (i == 1)
                        coeffcientA[i] = Q;
                    else
                        coeffcientA[i] = 0;
                }
            }
        }
        public float[] EqualizeWave()
        {
            float[] output = new float[AudioFile.Samples.Length];

            return output;
        }
    }
}

