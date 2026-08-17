using Kasane2D.Exceptions.Engine;

namespace Kasane2D.Sound.Types;

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
    public override AudioStream Append(AudioStream stream)
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