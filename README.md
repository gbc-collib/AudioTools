### AudioTools
## A C# library for loading audiofiles, converting their bitrates, and applying effects to them. The library is organized sub-directories for seperate functions for reading, saving, and editing wav files. Currently my development time is being focused on designing a scalable GUI for the two types of audio effects currently implemented that will allow users to arrange pedals in any way they desire and hear the changes on the fly. After this GUi framework is built focus will return to expanding the list of effects availiable.

## OverDrive Distortion Process
# AmplifySignal: 
This step increases the gain or volume of the audio signal, which can make the sound louder and potentially create distortion if the gain is set too high. Similar to to turning gain knob on an analog amp.

# ButtersworthLowPassFilter: 
This is a type of low-pass filter that attenuates frequencies above a certain cutoff frequency, allowing lower frequencies to pass through. This can help to smooth out the sound and reduce harshness, which can be useful for creating a distortion effect.

# SoftClipShaper: 
This step applies a soft-clipping function to the audio signal, which can create a distortion effect by limiting the range of the signal and causing it to "clip" at the upper and lower limits. This can give the sound a characteristic "crunchy" or "dirty" quality.

# ButtersworthHighPassFilter: 
This is a type of high-pass filter that attenuates frequencies below a certain cutoff frequency, allowing higher frequencies to pass through. This can help to reduce low-frequency noise or rumble, which can be useful for creating a distortion effect.
