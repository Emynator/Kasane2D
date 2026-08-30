using System.Diagnostics;
using Kasane2D.Sound.Interfaces;

namespace Kasane2D.Sound.Mixer;

internal class MixBus : IMixBus
{
    private readonly string systemKey;
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
    private float gain;
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
        systemKey = $"Engine::SoundSystem::MixBus::{name}::Mix";
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
        Gain = 0.0f;
        Pan = 0;
    }

    public string Name { get; }

    public float Gain
    {
        get => 20.0f * MathF.Log10(gain);
        set
        {
            tlock.Wait();
            var actual = MathF.Max(-60.0f, MathF.Min(20.0f, value));
            gain = MathF.Pow(10.0f, actual / 20.0f);
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
    }

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
    
    public List<MixBus> InternalChildren { get; set; } = [];

    public IReadOnlyCollection<IAudioEffect> Effects => effects;

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

    public void SetEffects(IReadOnlyCollection<IAudioEffect> effects)
    {
        tlock.Wait();
        this.effects = effects.ToList();
        tlock.Release();
    }

    public void ClearEffects()
    {
        tlock.Wait();
        effects.Clear();
        tlock.Release();
    }

    public void WriteLeft(ReadOnlySpan<float> samples)
    {
        tlock.Wait();
        inLeft.Write(samples);
        tlock.Release();
    }

    public void WriteRight(ReadOnlySpan<float> samples)
    {
        tlock.Wait();
        inRight.Write(samples);
        tlock.Release();
    }

    public void ReadLeft(Span<float> outBuffer)
    {
        tlock.Wait();
        outLeft.Read(outBuffer);
        tlock.Release();
    }

    public void ReadRight(Span<float> outBuffer)
    {
        tlock.Wait();
        outRight.Read(outBuffer);
        tlock.Release();
    }

    public void Mix()
    {
        tlock.Wait();
        Engine.Monitor.StartMeasurement(systemKey);

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
            if (effect.Bypass)
            {
                continue;
            }
            
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

        Engine.Monitor.FinishMeasurement(systemKey);
        tlock.Release();
    }
}