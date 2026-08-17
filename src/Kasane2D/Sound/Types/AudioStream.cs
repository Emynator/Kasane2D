using Kasane2D.Exceptions.Engine;

namespace Kasane2D.Sound.Types;

/// <summary>
/// Abstract base class to represent an arbitrary amount of audio data.
/// </summary>
public abstract record class AudioStream
{
    /// <summary>
    /// Creates a new audio stream.
    /// </summary>
    /// <param name="length">Length in samples.</param>
    protected AudioStream(int length)
    {
        Length = length;
    }

    /// <summary>
    /// Gets the stream length in samples.
    /// </summary>
    public int Length { get; }

    /// <summary>
    /// Resample the audio stream from the source sample rate to the destination sample rate.
    /// </summary>
    /// <param name="srcSampleRate">Source sample rate of the stream.</param>
    /// <param name="dstSampleRate">Destination sample rate to resample to.</param>
    /// <returns>The resampled stream.</returns>
    public abstract AudioStream Resample(int srcSampleRate, int dstSampleRate);

    /// <summary>
    /// Returns a slice of the audio stream.
    /// </summary>
    /// <param name="start">The start of the slice.</param>
    /// <param name="length">The length of the slice.</param>
    /// <returns>The sliced audio stream.</returns>
    public abstract AudioStream Slice(int start, int length);

    /// <summary>
    /// Appends another audio stream to this one.
    /// </summary>
    /// <param name="stream">The audio stream to add.</param>
    /// <returns>The resulting merged audio stream.</returns>
    public abstract AudioStream Append(AudioStream stream);

    /// <summary>
    /// Creates a copy of the audio stream.
    /// </summary>
    /// <returns>The copy.</returns>
    public abstract AudioStream Copy();

    /// <summary>
    /// Resampling function.
    /// </summary>
    /// <param name="src">Sample data to resample.</param>
    /// <param name="srcSampleRate">Sampler rate of the source data.</param>
    /// <param name="dstSampleRate">Sampler rate to resample to.</param>
    /// <returns>The resampled data.</returns>
    protected static float[] ResampleChannel(Span<float> src, int srcSampleRate, int dstSampleRate)
    {
        if (src.Length == 0)
        {
            return [];
        }

        var length = (int)MathF.Round(src.Length * (float)dstSampleRate / srcSampleRate);
        var result = new float[length];
        var sourceStep = (float)srcSampleRate / dstSampleRate;
        for (var i = 0; i < length; i++)
        {
            var sourcePosition = i * sourceStep;
            var sourceIndex = (int)sourcePosition;
            if (sourceIndex >= src.Length - 1)
            {
                result[i] = src[^1];

                continue;
            }

            var current = src[sourceIndex];
            var next = src[sourceIndex + 1];
            var fraction = sourcePosition - sourceIndex;
            result[i] = current + ((next - current) * fraction);
        }

        return result;
    }
}