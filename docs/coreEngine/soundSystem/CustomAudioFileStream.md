# 4.8.3 - Implementing custom Audio File Streams
The abstract `AudioFileStream` base class already handles all kinds of common operations for audio files. This includes resampling if required, handling pre-load and caching if desired, as well as managing and disposing the `BinaryReader` used to read the file from disk.

There are only 3 functionalities that the abstract class can't implement itself since they require knowledge of the actual file format. Those are the functions you need to override in your implementation. There are also a number of protected fields that you are expected to set.

# Method Overrides
`protected abstract byte[] ReadRawSamples(int sampleCount)` is the function the base class uses to read the raw sample data in bytes. For any file type that uses some form of compression or encoding scheme, this is the method where you need to implement the decoding of the encoded audio data to return the raw bytes of the sample data.
The `sampleCount` parameter refers to the requested number of samples to decode across all channels. So if your file contains only a mono audio stream, you decode `sampleCount` mono audio samples of that file. If your file contains more than one channel, you need to decode `sampleCount` samples for each channel.

`protected abstract AudioStream Convert(int sampleCount, Span<byte> rawData)` is the function the base class uses to convert raw byte data for samples into an actual audio stream. The format of this data is the raw format you decoded in `ReadRawSamples()`. It is required to convert the samples to 32-bit float format, regardless of the bitdepth that the underlying file format used.
The engine currently only supports mono and stereo audio. This means that you are free to discard the remaining audio channels.

`public virtual void SetPosition(int value)` is the function that is used to seek inside the audio stream. You should override this function and call the base implementation if you require to adjust the seek position inside the file in regards to file headers. This is only relevant in case the read mode is `AudioFileReadMode.Stream`.

# Required Fields and Properties
`sampleRate` is the actual sample rate of the audio file. This information is necessary for the base class to resample audio data to the sound system's sample rate.

`initDone` is an event signal for the pre-loading task. Once you have parsed the file header and have done your initialization, you should call `initDone.Set()` to signal the pre-load thread that it can start with pre-loading the audio file.

`Length` is the total length of the audio file in samples per channel. If the length is for example 44100 samples, that means a mono audio file contains 44100 mono samples and a stereo audio file contains 44100 samples per channel or 88200 samples in total.