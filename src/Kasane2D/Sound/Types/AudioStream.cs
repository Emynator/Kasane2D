namespace Kasane2D.Sound.Types;

public abstract record class AudioStream
{
    protected AudioStream(int length)
    {
        Length = length;
    }
    
    public int Length { get; }
    
    public abstract AudioStream Resample(int srcSampleRate, int dstSampleRate);

    public abstract AudioStream Slice(int start, int length);

    public abstract AudioStream Add(AudioStream stream);

    public abstract AudioStream Copy();

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

public sealed record class MonoAudioStream : AudioStream
{
    public static MonoAudioStream Empty { get; } = new MonoAudioStream(0, []);

    private readonly float[] samples;
    
    public MonoAudioStream(int length, float[] samples) : base(length)
    {
        if (samples.Length != length)
        {
            throw new InvalidOperationException();
        }
        
        this.samples = samples;
    }

    public ReadOnlySpan<float> GetSamples()
    {
        return samples.AsSpan();
    }
    
    public override AudioStream Resample(int srcSampleRate, int dstSampleRate)
    {
        var newSamples = ResampleChannel(samples, srcSampleRate, dstSampleRate);
        
        return new MonoAudioStream(newSamples.Length, newSamples);
    }

    public override AudioStream Slice(int start, int length)
    {
        var actualLength = Math.Min(length, samples.Length);
        var newSamples = new float[actualLength];
        samples.AsSpan(start, actualLength).CopyTo(newSamples);

        return new MonoAudioStream(actualLength, newSamples);
    }

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
                throw new InvalidOperationException();
        }

        return new MonoAudioStream(newSamples.Length, newSamples);
    }

    public override AudioStream Copy()
    {
        var newSamples = new float[Length];
        samples.AsSpan().CopyTo(newSamples);
        
        return new MonoAudioStream(Length, newSamples);
    }
    
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

public sealed record class StereoAudioStream : AudioStream
{
    public static StereoAudioStream Empty { get; } = new StereoAudioStream(0, [], []);
    
    private readonly float[] left;
    private readonly float[] right;

    public StereoAudioStream(int length, float[] left, float[] right) : base(length)
    {
        if (left.Length != length || right.Length != length)
        {
            throw new InvalidOperationException();
        }
        
        this.left = left;
        this.right = right;
    }

    public ReadOnlySpan<float> GetLeft()
    {
        return left.AsSpan();
    }

    public ReadOnlySpan<float> GetRight()
    {
        return right.AsSpan();
    }
    
    public override AudioStream Resample(int srcSampleRate, int dstSampleRate)
    {
        var newLeft = ResampleChannel(left, srcSampleRate, dstSampleRate);
        var newRight = ResampleChannel(right, dstSampleRate, dstSampleRate);
        
        return new StereoAudioStream(left.Length, newLeft, newRight);
    }

    public override AudioStream Slice(int start, int length)
    {
        var actualLength = Math.Min(length, Length);
        var newLeft = new float[actualLength];
        left.AsSpan(start, actualLength).CopyTo(newLeft);

        var newRight = new float[actualLength];
        right.AsSpan(start, actualLength).CopyTo(newRight);

        return new StereoAudioStream(actualLength, newLeft, newRight);
    }

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
                throw new InvalidOperationException();
        }
        
        return new StereoAudioStream(newLeft.Length, newLeft, newRight);
    }

    public override AudioStream Copy()
    {
        var newLeft = new float[Length];
        left.AsSpan().CopyTo(newLeft);
        
        var newRight = new float[Length];
        right.AsSpan().CopyTo(newRight);
        
        return new StereoAudioStream(Length, newLeft, newRight);
    }

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