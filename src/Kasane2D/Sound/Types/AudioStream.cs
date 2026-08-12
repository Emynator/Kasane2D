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
    public abstract AudioStream Add(AudioStream stream);

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

/// <summary>
/// A stream of mono audio data.
/// </summary>
/// <inheritdoc/>
public sealed record class MonoAudioStream : AudioStream
{
    /// <summary>
    /// An empty mono audio stream.
    /// </summary>
    public static MonoAudioStream Empty { get; } = new MonoAudioStream(0, []);

    private readonly float[] samples;

    /// <summary>
    /// Creates a new mono audio stream.
    /// </summary>
    /// <param name="length">The length of the stream in samples.</param>
    /// <param name="samples">The sample data.</param>
    /// <exception cref="DataConsistencyException">Thrown if length and sample data length are not equal.</exception>
    public MonoAudioStream(int length, float[] samples) : base(length)
    {
        if (samples.Length != length)
        {
            throw new DataConsistencyException($"{nameof(length)} must be equal to the length of {nameof(samples)}.");
        }

        this.samples = samples;
    }

    /// <summary>
    /// Gets the underlying sample data.
    /// </summary>
    /// <returns>The sample data.</returns>
    public ReadOnlySpan<float> GetSamples()
    {
        return samples.AsSpan();
    }

    /// <inheritdoc/>
    public override AudioStream Resample(int srcSampleRate, int dstSampleRate)
    {
        var newSamples = ResampleChannel(samples, srcSampleRate, dstSampleRate);

        return new MonoAudioStream(newSamples.Length, newSamples);
    }

    /// <inheritdoc/>
    public override AudioStream Slice(int start, int length)
    {
        var actualLength = Math.Min(length, samples.Length);
        var newSamples = new float[actualLength];
        samples.AsSpan(start, actualLength).CopyTo(newSamples);

        return new MonoAudioStream(actualLength, newSamples);
    }

    /// <inheritdoc/>
    public override AudioStream Add(AudioStream stream)
    {
        var newSamples = new float[Length + stream.Length];
        samples.AsSpan().CopyTo(newSamples);

        switch (stream)
        {
            case MonoAudioStream monoStream:
                monoStream.GetSamples().CopyTo(newSamples.AsSpan(Length, stream.Length));
                break;

            case StereoAudioStream stereoStream:
                var mono = stereoStream.ConvertToMono();
                mono.GetSamples().CopyTo(newSamples.AsSpan(Length, stream.Length));
                break;

            default:
                throw new ArgumentOutOfRangeException
                (
                    nameof(stream),
                    stream,
                    $"The stream must be either a {nameof(MonoAudioStream)} or a {nameof(StereoAudioStream)}."
                );
        }

        return new MonoAudioStream(newSamples.Length, newSamples);
    }

    /// <inheritdoc/>
    public override AudioStream Copy()
    {
        var newSamples = new float[Length];
        samples.AsSpan().CopyTo(newSamples);

        return new MonoAudioStream(Length, newSamples);
    }

    /// <summary>
    /// Converts the mono audio stream to a stereo audio stream.
    /// </summary>
    /// <returns>The resulting stereo audio stream.</returns>
    public StereoAudioStream ConvertToStereo()
    {
        var data = samples.AsSpan();
        var left = new float[Length];
        var right = new float[Length];

        data.CopyTo(left);
        data.CopyTo(right);

        return new StereoAudioStream(Length, left, right);
    }
}

/// <summary>
/// A stream of stereo audio data.
/// </summary>
public sealed record class StereoAudioStream : AudioStream
{
    /// <summary>
    /// An empty stereo audio stream.
    /// </summary>
    public static StereoAudioStream Empty { get; } = new StereoAudioStream(0, [], []);

    private readonly float[] left;
    private readonly float[] right;

    /// <summary>
    /// Creates a new stereo audio stream.
    /// </summary>
    /// <param name="length">The length of the stream in samples.</param>
    /// <param name="left">The sample data of the left channel.</param>
    /// <param name="right">The sample data of the right channel.</param>
    /// <exception cref="DataConsistencyException">Thrown if length and the lengths of the data arrays are not equal.</exception>
    public StereoAudioStream(int length, float[] left, float[] right) : base(length)
    {
        if (left.Length != length || right.Length != length)
        {
            throw new DataConsistencyException
                ($"{nameof(length)}, the length of {nameof(left)}, and the length of {nameof(right)} must be equal.");
        }

        this.left = left;
        this.right = right;
    }

    /// <summary>
    /// Gets the underlying sample data of the left channel.
    /// </summary>
    /// <returns>The sample data.</returns>
    public ReadOnlySpan<float> GetLeft()
    {
        return left.AsSpan();
    }

    /// <summary>
    /// Gets the underlying sample data of the right channel.
    /// </summary>
    /// <returns>The sample data.</returns>
    public ReadOnlySpan<float> GetRight()
    {
        return right.AsSpan();
    }

    /// <inheritdoc/>
    public override AudioStream Resample(int srcSampleRate, int dstSampleRate)
    {
        var newLeft = ResampleChannel(left, srcSampleRate, dstSampleRate);
        var newRight = ResampleChannel(right, dstSampleRate, dstSampleRate);

        return new StereoAudioStream(left.Length, newLeft, newRight);
    }

    /// <inheritdoc/>
    public override AudioStream Slice(int start, int length)
    {
        var actualLength = Math.Min(length, Length);
        var newLeft = new float[actualLength];
        left.AsSpan(start, actualLength).CopyTo(newLeft);

        var newRight = new float[actualLength];
        right.AsSpan(start, actualLength).CopyTo(newRight);

        return new StereoAudioStream(actualLength, newLeft, newRight);
    }

    /// <inheritdoc/>
    public override AudioStream Add(AudioStream stream)
    {
        var newLeft = new float[Length + stream.Length];
        left.AsSpan().CopyTo(newLeft);

        var newRight = new float[Length + stream.Length];
        right.AsSpan().CopyTo(newRight);

        switch (stream)
        {
            case MonoAudioStream monoStream:
                var converted = monoStream.ConvertToStereo();
                converted.GetLeft().CopyTo(newLeft.AsSpan(Length, stream.Length));
                converted.GetRight().CopyTo(newRight.AsSpan(Length, stream.Length));
                break;

            case StereoAudioStream stereoStream:
                stereoStream.GetLeft().CopyTo(newLeft.AsSpan(Length, stream.Length));
                stereoStream.GetRight().CopyTo(newRight.AsSpan(Length, stream.Length));
                break;

            default:
                throw new ArgumentOutOfRangeException
                (
                    nameof(stream),
                    stream,
                    $"The stream must be either a {nameof(MonoAudioStream)} or a {nameof(StereoAudioStream)}."
                );
        }

        return new StereoAudioStream(newLeft.Length, newLeft, newRight);
    }

    /// <inheritdoc/>
    public override AudioStream Copy()
    {
        var newLeft = new float[Length];
        left.AsSpan().CopyTo(newLeft);

        var newRight = new float[Length];
        right.AsSpan().CopyTo(newRight);

        return new StereoAudioStream(Length, newLeft, newRight);
    }

    /// <summary>
    /// Converts the stereo audio stream to a mono audio stream.
    /// </summary>
    /// <returns>The resulting mono audio stream.</returns>
    public MonoAudioStream ConvertToMono()
    {
        var samples = new float[Length];
        for (var i = 0; i < Length; i++)
        {
            samples[i] = (left[i] + right[i]) * 0.5f;
        }

        return new MonoAudioStream(samples.Length, samples);
    }
}