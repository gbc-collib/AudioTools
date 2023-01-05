### AudioTools
## A C# library for loading audiofiles, converting their bitrates, and applying effects to them. The library is organized sub-directories for seperate functions for reading, saving, and editing wav files

## OverDrive Distortion Process
#AmplifySignal: 
This step increases the gain or volume of the audio signal, which can make the sound louder and potentially create distortion if the gain is set too high.

# ButtersworthLowPassFilter: 
This is a type of low-pass filter that attenuates frequencies above a certain cutoff frequency, allowing lower frequencies to pass through. This can help to smooth out the sound and reduce harshness, which can be useful for creating a distortion effect.

# SoftClipShaper: 
This step applies a soft-clipping function to the audio signal, which can create a distortion effect by limiting the range of the signal and causing it to "clip" at the upper and lower limits. This can give the sound a characteristic "crunchy" or "dirty" quality.

# ButtersworthHighPassFilter: 
This is a type of high-pass filter that attenuates frequencies below a certain cutoff frequency, allowing higher frequencies to pass through. This can help to reduce low-frequency noise or rumble, which can be useful for creating a distortion effect.
