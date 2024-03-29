﻿using System;
using System.Xml;

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
    public static class Distortion
    {
        //Our first type of distortion being implemented to get that classic 90s punk sound
        public static void OverdriveDistortion(IAudioData audioFile, float gain, float lowPassCutoff, float highPassCutoff)
        {
            float[] output = new float[audioFile.Samples.Length];
            output = ApplyNoiseGate(output, -10, 100, audioFile.SampleRate);
            output = AmplifySignal(audioFile.Samples, gain);
            output = ButtersworthHighPassFilter(output, 3, highPassCutoff, audioFile.SampleRate);
            output = SoftClipShaper(output);
            output = ButtersworthLowPassFilter(output, 3, lowPassCutoff, audioFile.SampleRate);
            //output = ApplyNoiseGate(output, 1, 2, audioFile.SampleRate);
            audioFile.Samples = output;
        }
        //First step is to just amplify signal uniformly
        public static float[] AmplifySignal(float[] input, float gain)
        {
            float[] output = new float[input.Length];

            for (int i = 0; i < input.Length; i++)
            {
                output[i] = input[i] * gain;
            }
            return output;
        }
        /*The noise gate function works by comparing the envelope of the input 
         * signal (a smoothed version of the absolute value of the signal) to 
         * a threshold. When the envelope is above the threshold, the gate is 
         * open and the input signal is passed through unmodified. When the 
         * envelope is below the threshold, the gate is closed and the output 
         * is set to zero. The attack and release parameters control how quickly 
         * the gate responds to
         * changes in the input signal. The attack parameter determines how
         * quickly the gate opens when the envelope rises above the threshold,
         * and the release parameter determines how quickly the gate closes when
         * the envelope falls below the threshold.
         */
        public static float[] ApplyNoiseGate(float[] input, float threshold, float timeConstant, int sampleRate)
        {
            int length = input.Length;
            float[] output = new float[length];

            float envelope = 0.0f;
            float attack = (1.0f - envelope) / (timeConstant * sampleRate);
            float release = envelope / (timeConstant * sampleRate);

            for (int i = 0; i < length; i++)
            {
                float inputSample = Math.Abs(input[i]);
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
                    output[i] = input[i];
                }
                else
                {
                    output[i] = 0.0f;
                }
            }

            return output;
        }
        //The buttersworth low filter function is used to roll high end off audio
        //The order parameter referrs to how many 'poles' the function will have
        //Higher order filters will be more computationally intense but they will have steeper roll off
        //Differential equation for buttersworth filter is
        //y[n] = b[0]*x[n] + b[1]*x[n-1] + ... + b[M]*x[n-M] - a[1]*y[n-1] - ... - a[N]*y[n-N]
        //Translated into code we get the following function
        public static float[] ButtersworthLowPassFilter(float[] input, int order,
            float cutOffFrequency, float sampleRate)
        {
            float[] output = new float[input.Length];
            float[] coeffcientA = new float[order + 1];
            float[] coeffcientB = new float[order + 1];
            ComputeButtersWorthLowFilterCoeffcients(order, cutOffFrequency, sampleRate, ref coeffcientA, ref coeffcientB);
            for (int n = 0; n < input.Length; n++)
            {
                float y = 0;
                for (int i = 0; i <= order; i++)
                {
                    if (n - i >= 0)
                        y += coeffcientB[i] * input[n - i] - coeffcientA[i] * output[n - i];
                }
                output[n] = y;
            }
            return output;
        }
        /* This function calculates what is known as the coeffcients, you can
         * think of this like a delay except the coeffcient changes how much 
         * our previous sample will effect the current one. This function is only
         * meant to be used in the scope of the Lowfilter as the Highfilter has
         * different equation. I dont know the how and why of this equation besides
         * for high level stuff but wikipedia search of butterworh contains
         * a lot more information than I'm willing to write in this comment
         * 
         */
        public static void ComputeButtersWorthLowFilterCoeffcients(int order, float cutOffFrequency, float sampleRate,
            ref float[] coeffcientA, ref float[] coeffcientB)
        {
            for (int i = 0; i < order; i++)
            {
                //This is another part of Butterworth equation that normalizes audio roll the audio off
                //The volume of the audio will get lower the closer we get to the cutoff
                float normilizationFactor = (float)Math.Pow((2 * cutOffFrequency / sampleRate), i);
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
        /* This type of distortion function is what is known as wave shaping
         * which will just add a little controlled harshness back to the audio
         * I used a pretty simple equation for this one but really any kind of
         * polynomial or quardratic would work to differing effects */
        public static float[] SoftClipShaper(float[] input)
        {
            float[] output = new float[input.Length];
            for (int i = 0; i < input.Length; i++)
            {
                //output[i] = 1.5f * (input[i]) - 0.5f * (float)Math.Pow(input[i], 3);
                output[i] = input[i] / (1 + Math.Abs(input[i]));
            }
            return output;
        }
        /* Ah back to Dr. Butterworth
         * The filtering is implemented using a finite impulse response (FIR)
         * filter structure, in which the current output sample depends on a 
         * weighted sum of the current and past input samples and the past
         * output samples. This is the standard approach for implementing a 
         * digital filter.
         */
        public static float[] ButtersworthHighPassFilter(float[] input, int order,
            float cutOffFrequency, float sampleRate)
        {
            float[] output = new float[input.Length];
            float[] coeffcientB = new float[order + 1];
            float[] coeffcientA = new float[order + 1];
            ComputeButtersWorthHighFilterCoeffcients(order, cutOffFrequency, sampleRate, ref coeffcientB, ref coeffcientA);
            for (int n = 0; n < input.Length; n++)
            {
                float y = 0;
                for (int i = 0; i <= order; i++)
                {
                    if (n - i >= 0)
                        //
                        y += coeffcientB[i] * input[n - i] - coeffcientA[i] * output[n - i];
                }
                output[n] = y;
            }
            return output;
        }
        public static void ComputeButtersWorthHighFilterCoeffcients(int order, float cutOffFrequency, float sampleRate,
            ref float[] coeffcientA, ref float[] coeffcientB)
        {
            {
                //Q in the transfer formula represents how fast the volume drops as we approach the cutoff
                float Q = (float)(Math.Sqrt(Math.Pow(2, 1.0 / order) - 1) / (Math.Pow(2, 1.0 / order) - 1));
                for (int i = 0; i <= order; i++)
                {
                    // This is another part of Butterworth equation that normalizes the audio roll-off
                    // The volume of the audio will get lower the closer we get to the cutoff
                    float normilizationFactor = (float)Math.Pow((2 * cutOffFrequency / sampleRate), i);
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
        public static float[] EqualizeWave(float[] input)
        {
            float[] output = new float[input.Length];

            return output;
        }
    }
}

