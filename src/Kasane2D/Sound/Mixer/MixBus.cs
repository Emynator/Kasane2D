using Kasane2D.Sound.Interfaces;

namespace Kasane2D.Sound.Mixer;

internal class MixBus : IMixBus
{
    private readonly SemaphoreSlim tlock = new(1, 1);
    
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
        OutLeft = outLeft;
        OutRight = outRight;
        InLeft = inLeft;
        InRight = inRight;
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

    public IAudioBuffer OutLeft { get; }

    public IAudioBuffer OutRight { get; }

    public IAudioBuffer InLeft { get; }

    public IAudioBuffer InRight { get; }

    public IMixBus? Parent => InternalParent;

    public MixBus? InternalParent { get; set; }

    public IReadOnlyCollection<IMixBus> Children => InternalChildren;
    
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

        var sumLeft = InternalChildren
            .Select(c => c.OutLeft.Read(sampleCount))
            .Aggregate
            (
                new float[sampleCount],
                (sum, next) =>
                {
                    for (var i = 0; i < sum.Length; i++)
                    {
                        sum[i] += next[i];
                    }

                    return sum;
                }
            );

        var sumRight = InternalChildren
            .Select(c => c.OutRight.Read(sampleCount))
            .Aggregate
            (
                new float[sampleCount],
                (sum, next) =>
                {
                    for (var i = 0; i < sum.Length; i++)
                    {
                        sum[i] += next[i];
                    }

                    return sum;
                }
            );

        var inLeft = InLeft.Read(sampleCount);
        var inRight = InRight.Read(sampleCount);
        for (var i = 0; i < sampleCount; i++)
        {
            sumLeft[i] += inLeft[i];
            sumRight[i] += inRight[i];
        }

        foreach (var effect in Effects)
        {
            effect.Apply(sumLeft, sumRight);
        }

        for (var i = 0; i < sampleCount; i++)
        {
            sumLeft[i] *= gain * leftGain;
            sumRight[i] *= gain * rightGain;
        }

        OutLeft.Write(sumLeft);
        OutRight.Write(sumRight);
        
        tlock.Release();
    }
}