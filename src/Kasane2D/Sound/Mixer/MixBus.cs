using System.Buffers;
using Kasane2D.Sound.Interfaces;

namespace Kasane2D.Sound.Mixer;

internal class MixBus : IMixBus
{
    private readonly SemaphoreSlim tlock = new(1, 1);
    private readonly int bufferSize;
    private readonly AudioBuffer outLeft;
    private readonly AudioBuffer outRight;
    private readonly AudioBuffer inLeft;
    private readonly AudioBuffer inRight;
    private readonly float[] scratchBuffer0;
    private readonly float[] scratchBuffer1;
    private readonly float[] scratchBuffer2;
    private readonly float[] scratchBuffer3;

    private List<IAudioEffect> effects = [];
    private float gain = 1.0f;
    private float leftGain = 1.0f;
    private float rightGain = 1.0f;

    public MixBus
        (
        string name,
        int bufferSize,
        AudioBuffer outLeft,
        AudioBuffer outRight,
        AudioBuffer inLeft,
        AudioBuffer inRight,
        MixBus? parent
        )
    {
        Name = name;
        this.bufferSize = bufferSize;
        this.outLeft = outLeft;
        this.outRight = outRight;
        this.inLeft = inLeft;
        this.inRight = inRight;
        scratchBuffer0 = new float[bufferSize];
        scratchBuffer1 = new float[bufferSize];
        scratchBuffer2 = new float[bufferSize];
        scratchBuffer3 = new float[bufferSize];
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

    public void Mix()
    {
        tlock.Wait();

        if (InternalChildren.Count >= 5)
        {
            Parallel.ForEach(InternalChildren, child => child.Mix());
        }
        else
        {
            foreach (var child in InternalChildren)
            {
                child.Mix();
            }
        }
        
        var left = scratchBuffer0.AsMemory();
        var sumLeft = scratchBuffer1.AsMemory();
        sumLeft.Span.Clear();
        var taskLeft = Task.Run(() =>
        {
            foreach (var child in InternalChildren)
            {
                child.outLeft.Read(left.Span);
                for (var i = 0; i < bufferSize; i++)
                {
                    sumLeft.Span[i] += left.Span[i];
                }
            }
        });

        var right = scratchBuffer2.AsMemory();
        var sumRight = scratchBuffer3.AsMemory();
        sumRight.Span.Clear();
        var taskRight = Task.Run(() =>
        {
            foreach (var child in InternalChildren)
            {
                child.outRight.Read(right.Span);
                for (var i = 0; i < bufferSize; i++)
                {
                    sumRight.Span[i] += right.Span[i];
                }
            }
        });

        Task.WaitAll(taskLeft, taskRight);
        
        inLeft.Read(left.Span);
        inRight.Read(right.Span);
        for (var i = 0; i < bufferSize; i++)
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

            var t = effectInLeft;
            effectInLeft = effectOutLeft;
            effectOutLeft = t;
            
            t = effectInRight;
            effectInRight = effectOutRight;
            effectOutRight = t;
        }

        for (var i = 0; i < bufferSize; i++)
        {
            effectInLeft[i] *= gain * leftGain;
            effectInRight[i] *= gain * rightGain;
        }

        outLeft.Write(effectInLeft);
        outRight.Write(effectInRight);

        tlock.Release();
    }
}