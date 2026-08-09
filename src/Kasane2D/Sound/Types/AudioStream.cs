namespace Kasane2D.Sound.Types;

public abstract record class AudioStream(int Length)
{
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

public sealed record class MonoAudioStream(int Length, float[] Samples) : AudioStream(Length)
{
    public static MonoAudioStream Empty { get; } = new MonoAudioStream(0, []);
    
    public override AudioStream Resample(int srcSampleRate, int dstSampleRate)
    {
        var samples = ResampleChannel(Samples, srcSampleRate, dstSampleRate);
        
        return new MonoAudioStream(samples.Length, samples);
    }

    public override AudioStream Slice(int start, int length)
    {
        var samples = new float[length];
        Samples.AsSpan(start, length).CopyTo(samples);

        return new MonoAudioStream(length, samples);
    }

    public override AudioStream Add(AudioStream stream)
    {
        var samples = new float[Length + stream.Length];
        Samples.AsSpan().CopyTo(samples);

        if (stream is MonoAudioStream monoStream)
        {
            monoStream.Samples.AsSpan().CopyTo(samples.AsSpan(Length, stream.Length));
        }

        if (stream is StereoAudioStream stereoStream)
        {
            var mono = new float[stream.Length];
            for (var i = 0; i < mono.Length; i++)
            {
                mono[i] = (stereoStream.Left[i] + stereoStream.Right[i]) / 2.0f;
            }

            mono.AsSpan().CopyTo(samples.AsSpan(Length, stream.Length));
        }

        return new MonoAudioStream(samples.Length, samples);
    }

    public override AudioStream Copy()
    {
        var samples = new float[Length];
        Samples.AsSpan().CopyTo(samples);
        
        return new MonoAudioStream(Length, samples);
    }
    
    public StereoAudioStream ConvertToStereo()
    {
        var data = Samples.AsSpan();
        var left = new float[Length];
        var right = new float[Length];
        
        data.CopyTo(left);
        data.CopyTo(right);
        
        return new StereoAudioStream(Length, left, right);
    }
}

public sealed record class StereoAudioStream(int Length, float[] Left, float[] Right) : AudioStream(Length)
{
    public static StereoAudioStream Empty { get; } = new StereoAudioStream(0, [], []);
    
    public override AudioStream Resample(int srcSampleRate, int dstSampleRate)
    {
        var left = ResampleChannel(Left, srcSampleRate, dstSampleRate);
        var right = ResampleChannel(Right, dstSampleRate, dstSampleRate);
        
        return new StereoAudioStream(Left.Length, left, right);
    }

    public override AudioStream Slice(int start, int length)
    {
        var left = new float[length];
        Left.AsSpan(start, length).CopyTo(left);

        var right = new float[length];
        Right.AsSpan(start, length).CopyTo(right);

        return new StereoAudioStream(length, left, right);
    }

    public override AudioStream Add(AudioStream stream)
    {
        var left = new float[Length + stream.Length];
        Left.AsSpan().CopyTo(left);

        var right = new float[Length + stream.Length];
        Right.AsSpan().CopyTo(right);

        if (stream is MonoAudioStream monoStream)
        {
            monoStream.Samples.AsSpan().CopyTo(left.AsSpan(Length, stream.Length));
            monoStream.Samples.AsSpan().CopyTo(right.AsSpan(Length, stream.Length));
        }

        if (stream is StereoAudioStream stereoStream)
        {
            stereoStream.Left.AsSpan().CopyTo(left.AsSpan(Length, stream.Length));
            stereoStream.Right.AsSpan().CopyTo(right.AsSpan(Length, stream.Length));
        }

        return new StereoAudioStream(left.Length, left, right);
    }

    public override AudioStream Copy()
    {
        var samplesLeft = new float[Length];
        Left.AsSpan().CopyTo(samplesLeft);
        
        var samplesRight = new float[Length];
        Right.AsSpan().CopyTo(samplesRight);
        
        return new StereoAudioStream(Length, samplesLeft, samplesRight);
    }

    public MonoAudioStream ConvertToMono()
    {
        var samples = new float[Length];
        for (var i = 0; i < Length; i++)
        {
            samples[i] = (Left[i] + Right[i]) * 0.5f;
        }
        
        return new MonoAudioStream(samples.Length, samples);
    }
}