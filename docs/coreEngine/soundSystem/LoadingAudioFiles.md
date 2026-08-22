# 4.4 - Loading Audio Files
Before audio files can be used for playback in the sound effect manager or the music player, they need to be loaded from the file system.
The engine provides an abstract `AudioFileStream` class that represents audio files loaded by the engine. Specialised implementations derive from this class to handle various different file formats.
`AudioFileStream`s are automatically converted by the engine to fit the sound system's sample rate and the 32-bit float mixer engine, regardless of the file's actual sample rate or bit depth.

Currently, there is only and implementation for the RIFF/WAVE file format available (also known by their file extension as `.wav` files). To load a RIFF/WAVE file, just create a new `WaveFileStream` and provide the (relative) file path, the sound system's sample rate, and optionally the read mode of the file.

The `AudioFileReadMode` determines how loading of audio files is handled:
- `AudioFileReadMode.Preload` means that the entire file will be read into RAM at once.
- `AudioFileReadMode.Stream` means that the file will remain on disk and audio data will be streamed from disk on demand.
- `AudioFileReadMode.CachedStream` means that the file will initially remain on disk, but every part of the file that is loaded will remain in RAM from that point on.

For implementing custom `AudioFileStream`s for various other audio formats, please refer to [the documentation for that](CustomAudioFileStream.md).