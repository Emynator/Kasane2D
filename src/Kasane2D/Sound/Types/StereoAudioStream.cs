using Kasane2D.Exceptions.Engine;

namespace Kasane2D.Sound.Types;

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
        var newRight = ResampleChannel(right, srcSampleRate, dstSampleRate);

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
    public override AudioStream Append(AudioStream stream)
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