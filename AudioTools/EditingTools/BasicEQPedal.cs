using AudioTools.AudioFileTools;

namespace AudioTools.EditingTools
{
	public class ButterWorthFilterEQ : IPedal
	{
        public IAudioData AudioFile { get; set; }
        public Dictionary<string, float> Parameters { get; set; }
        public bool IsEnabled { get; set; }
        public float LowPassCutOff
        {
            get { return Parameters["LowPassCutOff"]; }
            set { Parameters["LowPassCutOff"] = value; }
        }
        public float HighPassCutOff
        {
            get { return Parameters["HighPassFilter"]; }
            set { Parameters["HighPassFilter"] = value; }
        }

        public ButterWorthFilterEQ(IAudioData audioData, float lowPassCutOff, float highPassCutOff, float rate)
        {
            AudioFile = audioData;
            Parameters = new Dictionary<string, float>
            {
                {"MixPercent",  lowPassCutOff },
                {"Depth", highPassCutOff }
            };
            HighPassCutOff = highPassCutOff;
            LowPassCutOff = lowPassCutOff;
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

        public static int Factorial(int input)
        {
            int output = 1;
            for (int i = 1; i <= input; i++)
            {
                output = output * i;
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
        public void ApplyEffect()
        {
            throw new NotImplementedException();
        }
    }
}