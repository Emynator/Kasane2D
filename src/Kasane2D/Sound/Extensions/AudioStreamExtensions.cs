using Kasane2D.Sound.Types;

namespace Kasane2D.Sound.Extensions;

/// <summary>
/// Extension methods for <see cref="AudioStream"/>s
/// </summary>
public static class AudioStreamExtensions
{
    /// <summary>
    /// Gets the AudioStream as a StereoAudioStream.
    /// </summary>
    /// <param name="stream">The AudioStream to get as a StereoAudioStream.</param>
    /// <returns>The stream if it is already a StereoAudioStream or the MonoAudioStream converted to stereo.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the stream is neither a stereo nor a mono stream.</exception>
    public static StereoAudioStream AsStereoStream(this AudioStream stream)
    {
        if (stream is StereoAudioStream stereoStream)
        {
            return stereoStream;
        }

        return stream is MonoAudioStream monoStream
            ? monoStream.ConvertToStereo()
            : throw new InvalidOperationException("Stream is neither mono nor stereo!");
    }

    /// <summary>
    /// Gets the AudioStream as a MonoAudioStream.
    /// </summary>
    /// <param name="stream">The AudioStream to get as a MonoAudioStream.</param>
    /// <returns>The stream if it is already a MonoAudioStream or the StereoAudioStream converted to stereo.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the stream is neither a stereo nor a mono stream.</exception>
    public static MonoAudioStream AsMonoStream(this AudioStream stream)
    {
        if (stream is MonoAudioStream monoStream)
        {
            return monoStream;
        }
        
        return stream is StereoAudioStream stereoStream
            ? stereoStream.ConvertToMono()
            : throw new InvalidOperationException("Stream is neither mono nor stereo!");
    }
}