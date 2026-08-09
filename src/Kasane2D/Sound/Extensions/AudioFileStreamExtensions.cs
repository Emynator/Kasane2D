using Kasane2D.Sound.Types;

namespace Kasane2D.Sound.Extensions;

internal static class AudioFileStreamExtensions
{
    public static StereoAudioStream ReadIn(this AudioFileStream file)
    {
        file.Reset();
        var stream = file.Read(file.Length);
        if (stream is StereoAudioStream stereoStream)
        {
            return stereoStream;
        }

        return stream is MonoAudioStream monoStream
            ? monoStream.ConvertToStereo()
            : throw new ArgumentException("file is neither Stereo nor Mono.", nameof(file));
    }
}