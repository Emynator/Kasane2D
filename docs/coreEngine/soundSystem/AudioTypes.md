# 4.8.1 - Low-Level Audio Types
Kasane2D exposes various low-level types for dealing with audio data that are quite useful for advanced audio processing scenarios.

# IAudioBuffer
An `IAudioBuffer` is a fixed size ring buffer with two independent pointers for reading samples from and writing samples to the buffer.
You can picture it like a queue of a fixed size. Sample data goes in on one side and comes out in the same order on the other side.

Unlike a regular queue, though, audio buffers can overrun. If samples are read faster from the buffer than they are written to it, old sample data is read again. If samples are written faster to the buffer than they are read from it, samples get overwritten before they have flown out of the buffer.

Audio buffers are useful for various low-level audio processing tasks, but reading and writing needs to be carefully synchronized to prevent overruns or underfeeding the buffer.

The sound system's `IAudioBuffer` interface provides a thread safe audio buffer implementation for stereo audio buffers. Audio buffers can be created with `ISoundSystem.CreateBuffer()`.

# AudioStream
An `AudioStream` in Kasane2D is the abstract representation of a fixed-size chunk of audio samples. Audio streams provide operations for appending, slicing, and resampling the underlying audio data. The two concrete implementations are `MonoAudioStream` and `StereoAudioStream`. They provide access to the underlying sample data as well as conversion functionality to convert a mono stream to a stereo stream and vice versa.