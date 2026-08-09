using System.Buffers;
using Kasane2D.Sound.Interfaces;

namespace Kasane2D.Sound.Mixer;

internal class MixBus : IMixBus
{
    private static readonly ArrayPool<float> bufferPool = ArrayPool<float>.Shared;
    
    private readonly SemaphoreSlim tlock = new(1, 1);
    private readonly AudioBuffer outLeft;
    private readonly AudioBuffer outRight;
    private readonly AudioBuffer inLeft;
    private readonly AudioBuffer inRight;

    private List<IAudioEffect> effects = [];
    private float gain = 1.0f;
    private float leftGain = 1.0f;
    private float rightGain = 1.0f;

    public MixBus
        (
        string name,
        AudioBuffer outLeft,
        AudioBuffer outRight,
        AudioBuffer inLeft,
        AudioBuffer inRight,
        MixBus? parent
        )
    {
        Name = name;
        this.outLeft = outLeft;
        this.outRight = outRight;
        this.inLeft = inLeft;
        this.inRight = inRight;
        InternalParent = parent;
        parent?.InternalChildren.Add(this);
    }

    public string Name { get; }

    public int Level
    {
        get => (int)(20.0f * MathF.Log10(gain));
        set
        {
            tlock.Wait();
            gain = MathF.Pow(10.0f, value / 20.0f);
            tlock.Release();
        }
    }

    public int Pan
    {
        get;
        set
        {
            tlock.Wait();

            var actual = value;
            if (value > 100)
            {
                actual = 100;
            }
            if (value < -100)
            {
                actual = -100;
            }
            field = actual;

            var normalized = (1.0f / 100 * actual + 1.0f) * 0.5f;
            var angle = normalized * MathF.PI * 0.5f;
            leftGain = MathF.Cos(angle);
            rightGain = MathF.Sin(angle);

            tlock.Release();
        }
    } = 0;

    public IMixBus? Parent => InternalParent;

    public MixBus? InternalParent
    {
        get;
        set
        {
            tlock.Wait();
            field = value;
            tlock.Release();
        }
    }

    public IReadOnlyCollection<IMixBus> Children => InternalChildren;

    public IReadOnlyCollection<IAudioEffect> Effects => effects;

    public void WriteLeft(ReadOnlySpan<float> samples)
    {
        inLeft.Write(samples);
    }

    public void WriteRight(ReadOnlySpan<float> samples)
    {
        inRight.Write(samples);
    }

    public float[] ReadLeft(int sampleCount)
    {
        return outLeft.Read(sampleCount);
    }

    public float[] ReadRight(int sampleCount)
    {
        return outRight.Read(sampleCount);
    }

    public void AddEffect(IAudioEffect effect)
    {
        tlock.Wait();
        effects.Add(effect);
        tlock.Release();
    }

    public void RemoveEffect(string name)
    {
        tlock.Wait();
        effects = effects.Where(e => e.Name != name).ToList();
        tlock.Release();
    }

    public List<MixBus> InternalChildren { get; set; } = [];

    public void Mix(int sampleCount)
    {
        tlock.Wait();

        if (InternalChildren.Count >= 5)
        {
            Parallel.ForEach(InternalChildren, child => child.Mix(sampleCount));
        }
        else
        {
            foreach (var child in InternalChildren)
            {
                child.Mix(sampleCount);
            }
        }
        
        var b0 = bufferPool.Rent(sampleCount);
        var b1 = bufferPool.Rent(sampleCount);
        var b2 = bufferPool.Rent(sampleCount);
        var b3 = bufferPool.Rent(sampleCount);
        
        var left = b0.AsMemory().Slice(0, sampleCount);
        var sumLeft = b1.AsMemory().Slice(0, sampleCount);
        var taskLeft = Task.Run(() =>
        {
            foreach (var child in InternalChildren)
            {
                child.outLeft.Read(left.Span);
                for (var i = 0; i < sampleCount; i++)
                {
                    sumLeft.Span[i] += left.Span[i];
                }
            }
        });

        var right = b2.AsMemory().Slice(0, sampleCount);
        var sumRight = b3.AsMemory().Slice(0, sampleCount);
        var taskRight = Task.Run(() =>
        {
            foreach (var child in InternalChildren)
            {
                child.outRight.Read(right.Span);
                for (var i = 0; i < sampleCount; i++)
                {
                    sumRight.Span[i] += right.Span[i];
                }
            }
        });

        Task.WaitAll(taskLeft, taskRight);
        
        inLeft.Read(left.Span);
        inRight.Read(right.Span);
        for (var i = 0; i < sampleCount; i++)
        {
            sumLeft.Span[i] += left.Span[i];
            sumRight.Span[i] += right.Span[i];
        }

        var effectInLeft = sumLeft.Span;
        var effectInRight = sumRight.Span;
        var effectOutLeft = left.Span;
        var effectOutRight = right.Span;
        foreach (var effect in Effects)
        {
            effect.Apply(effectInLeft, effectInRight, effectOutLeft, effectOutRight);

            var l = effectInLeft;
            effectInLeft = effectOutLeft;
            effectOutLeft = l;

            var r = effectInRight;
            effectInRight = effectOutRight;
            effectOutRight = r;
        }

        for (var i = 0; i < sampleCount; i++)
        {
            effectInLeft[i] *= gain * leftGain;
            effectInRight[i] *= gain * rightGain;
        }

        outLeft.Write(effectInLeft);
        outRight.Write(effectInRight);

        bufferPool.Return(b0);
        bufferPool.Return(b1);
        bufferPool.Return(b2);
        bufferPool.Return(b3);

        tlock.Release();
    }
}